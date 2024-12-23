using Cunet.Protocol.Packets.Status.Client;
using Cunet.Protocol.Primitive;

namespace Cunet.Protocol.Packets.Status.Server;

/// <summary>
///     Represents the server-bound ping request packet sent by the client to calculate its latency to the server. The
///     server responds with <see cref="PongResponsePacket" /> with <see cref="PongResponsePacket.Payload" /> set to the
///     same value as <see cref="Payload" /> in this request packet.
/// </summary>
public readonly struct PingRequestPacket : IServerBoundPacket<PingRequestPacket> {
    /// <summary>
    ///     An arbitrary payload. The server will include this payload in its response. The Notchian client uses the current
    ///     UNIX timestamp (milliseconds).
    /// </summary>
    public required Long Payload { get; init; }

    public int CalculateSize() {
        return Payload.CalculateSize();
    }

    public int Write(Span<byte> output) {
        return Payload.Write(output);
    }

    public static PingRequestPacket Read(ReadOnlySpan<byte> input, out int consumed) {
        Long payload = Long.Read(input, out consumed);
        return new PingRequestPacket {
            Payload = payload,
        };
    }
}
