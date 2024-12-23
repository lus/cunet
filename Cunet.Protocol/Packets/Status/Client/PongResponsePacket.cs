using Cunet.Protocol.Packets.Status.Server;
using Cunet.Protocol.Primitive;

namespace Cunet.Protocol.Packets.Status.Client;

/// <summary>
///     Represents the client-bound pong response packet sent by the server after receiving a
///     <see cref="PingRequestPacket" />. The <see cref="Payload" /> equals the <see cref="PingRequestPacket.Payload" />
///     value received from the client.
/// </summary>
public readonly struct PongResponsePacket : IClientBoundPacket<PongResponsePacket> {
    /// <summary>
    ///     The same payload as received in the <see cref="PingRequestPacket" />.
    /// </summary>
    public required Long Payload { get; init; }

    public int CalculateSize() {
        return Payload.CalculateSize();
    }

    public int Write(Span<byte> output) {
        return Payload.Write(output);
    }

    public static PongResponsePacket Read(ReadOnlySpan<byte> input, out int consumed) {
        Long payload = Long.Read(input, out consumed);
        return new PongResponsePacket {
            Payload = payload,
        };
    }
}
