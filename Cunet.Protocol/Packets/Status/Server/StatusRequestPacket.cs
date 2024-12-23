using Cunet.Protocol.Packets.Handshaking.Server;
using Cunet.Protocol.Packets.Status.Client;

namespace Cunet.Protocol.Packets.Status.Server;

/// <summary>
///     Represents the server-bound status request packet sent by the client once immediately after the
///     <see cref="HandshakePacket" /> with <see cref="HandshakePacket.State" /> set to
///     <see cref="HandshakePacket.NextState.Status" /> requesting the status information from the server.
///     The server responds with <see cref="StatusResponsePacket" />.
/// </summary>
public readonly struct StatusRequestPacket : IServerBoundPacket<StatusRequestPacket> {
    public int CalculateSize() {
        return 0;
    }

    public int Write(Span<byte> output) {
        return 0;
    }

    public static StatusRequestPacket Read(ReadOnlySpan<byte> input, out int consumed) {
        consumed = 0;
        return new StatusRequestPacket();
    }
}
