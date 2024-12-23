using Cunet.Protocol.Primitive;
using Cunet.Protocol.Serialization;
using Cunet.Protocol.Session;
using JetBrains.Annotations;
using String = Cunet.Protocol.Primitive.String;

namespace Cunet.Protocol.Packets.Handshaking.Server;

/// <summary>
///     Represents the server-bound handshaking packet sent by the client whenever opening a new TCP connection.
/// </summary>
public readonly struct HandshakePacket : IServerBoundPacket<HandshakePacket> {
    /// <summary>
    ///     The protocol version of the client opening the connection.
    /// </summary>
    public required int ProtocolVersion { get; init; }

    /// <summary>
    ///     The address the client used to connect to the server.
    /// </summary>
    public required string ServerAddress { get; init; }

    /// <summary>
    ///     The port the client used to connect to the server.
    /// </summary>
    public required ushort ServerPort { get; init; }

    /// <summary>
    ///     The state the client wants to switch to.
    /// </summary>
    public required NextState State { get; init; }

    public int CalculateSize() {
        return new VarInt(ProtocolVersion).CalculateSize() +
               new String(ServerAddress).CalculateSize() +
               new UnsignedShort(ServerPort).CalculateSize() +
               new VarInt((int)State).CalculateSize();
    }

    public int Write(Span<byte> output) {
        ProtocolWriter writer = new(output);
        writer.Write(new VarInt(ProtocolVersion))
            .Write(new String(ServerAddress))
            .Write(new UnsignedShort(ServerPort))
            .Write(new VarInt((int)State));
        return writer.TotalWritten;
    }

    public static HandshakePacket Read(ReadOnlySpan<byte> input, out int consumed) {
        ProtocolReader reader = new(input);

        int protocolVersion = reader.Read<VarInt>().Value;
        string serverAddress = reader.Read<String>().Value;
        ushort serverPort = reader.Read<UnsignedShort>().Value;
        int state = reader.Read<VarInt>().Value;
        if (state is < 1 or > 3) {
            throw new ReadException($"got invalid next state after handshake ({state})");
        }

        consumed = reader.TotalConsumed;
        return new HandshakePacket {
            ProtocolVersion = protocolVersion,
            ServerAddress = serverAddress,
            ServerPort = serverPort,
            State = (NextState)state,
        };
    }

    /// <summary>
    ///     Represents the <see cref="SessionState" /> the client wants to switch to in a <see cref="HandshakePacket" />.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public enum NextState {
        Status = 1,
        Login = 2,
        Transfer = 3,
    }
}
