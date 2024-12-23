using Cunet.Protocol.Packets;
using JetBrains.Annotations;

namespace Cunet.Protocol.Networking;

/// <summary>
///     Represents a pipe used to send and receive Minecraft protocol packets.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers | ImplicitUseTargetFlags.WithInheritors)]
public interface IPacketPipe {
    /// <summary>
    ///     Sends a packet to the receiving end of this pipe.
    /// </summary>
    /// <param name="packet">The packet to send.</param>
    /// <typeparam name="TPacket">The type of the packet to send.</typeparam>
    public Task SendPacketAsync<TPacket>(TPacket packet) where TPacket : IPacket;

    /// <summary>
    ///     Asynchronously waits for the next packet to be received.
    ///     This method may return <c>null</c> if the pipe is closed or an unknown packet is received.
    /// </summary>
    /// <returns>The received packet.</returns>
    public Task<IPacket?> ReceivePacketAsync();
}
