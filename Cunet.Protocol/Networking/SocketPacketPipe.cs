using System.Buffers;
using System.IO.Pipelines;
using System.Net.Sockets;
using Cunet.Protocol.Packets;
using Cunet.Protocol.Primitive;
using Cunet.Protocol.Serialization;
using Cunet.Protocol.Session;
using JetBrains.Annotations;
using Pipelines.Sockets.Unofficial;

namespace Cunet.Protocol.Networking;

/// <summary>
///     Represents a <see cref="IPacketPipe" /> implemented using a TCP <see cref="Socket" /> and
///     <see cref="SocketConnection" />.
/// </summary>
/// <param name="socket">The socket to use for communication.</param>
/// <param name="packetRegistry">The packet registry to use for looking up packet IDs and suppliers.</param>
/// <param name="role">
///     The role of the session this pipe belongs to. If we are a server talking to a client using this pipe, this is set
///     to <see cref="SessionRole.Server" />.
/// </param>
public class SocketPacketPipe(Socket socket, IPacketRegistry packetRegistry, SocketPacketPipe.SessionRole role)
    : IPacketPipe {
    /// <summary>
    ///     Represents the role of a session.
    ///     A session belonging to a client talking to a server has the <see cref="Client" /> role.
    ///     A session belonging to a server talking to a client has the <see cref="Server" /> role.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public enum SessionRole {
        Client,
        Server,
    }

    private SocketConnection _connection = SocketConnection.Create(socket);

    /// <summary>
    ///     The current state of the session belonging to the packet pipe. This is required for looking up the correct packet
    ///     IDs and suppliers using the provided <see cref="IPacketRegistry" />.
    /// </summary>
    public SessionState SessionState { get; set; } = SessionState.Handshaking;

    public async Task SendPacketAsync<TPacket>(TPacket packet) where TPacket : IPacket {
        int? packetId = role == SessionRole.Client
            ? packetRegistry.FindServerBoundPacketId(SessionState, typeof(TPacket))
            : packetRegistry.FindClientBoundPacketId(SessionState, typeof(TPacket));

        if (packetId == null) {
            return;
        }

        int size = packet.CalculateSize();

        VarInt packetIdElement = new(packetId.Value);
        size += packetIdElement.CalculateSize();

        VarInt sizePrefixElement = new(size);
        size += sizePrefixElement.CalculateSize();

        Memory<byte> mem = _connection.Output.GetMemory(size);
        int written = WriteElements(mem.Span, sizePrefixElement, packetIdElement, packet);
        _connection.Output.Advance(written);

        await _connection.Output.FlushAsync();
    }

    public async Task<IPacket?> ReceivePacketAsync() {
        while (_connection.ShutdownKind == PipeShutdownKind.None) {
            ReadResult result = await _connection.Input.ReadAsync();
            if (!TryReadPacket(result.Buffer, out IPacket? packet, out int consumed)) {
                continue;
            }

            _connection.Input.AdvanceTo(result.Buffer.GetPosition(consumed));
            return packet;
        }

        return null;
    }

    private bool TryReadPacket(ReadOnlySequence<byte> input, out IPacket? packet, out int consumed) {
        packet = null;
        consumed = 0;

        if (!VarInt.TryRead(input, out VarInt? sizeVarInt, out int consumedBySize)) {
            return false;
        }

        int size = sizeVarInt!.Value.Value;
        ReadOnlySequence<byte> remaining = input.Slice(consumedBySize);

        if (remaining.Length < size) {
            return false;
        }

        Span<byte> output = stackalloc byte[size];
        remaining.Slice(0, size).CopyTo(output);

        int packetId = VarInt.Read(output, out int consumedByPacketId).Value;

        IPacketRegistry.PacketSupplier? supplier = role == SessionRole.Client
            ? packetRegistry.FindClientBoundPacketSupplier(SessionState, packetId)
            : packetRegistry.FindServerBoundPacketSupplier(SessionState, packetId);

        packet = supplier?.Invoke(output[consumedByPacketId..], out _);
        consumed = size + consumedBySize;
        return true;
    }

    private static int WriteElements(Span<byte> target, params IWritableProtocolElement[] elements) {
        ProtocolWriter writer = new(target);
        foreach (IWritableProtocolElement element in elements) {
            writer.Write(element);
        }
        return writer.TotalWritten;
    }
}
