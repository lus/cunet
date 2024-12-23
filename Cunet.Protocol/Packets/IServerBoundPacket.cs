using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Packets;

/// <summary>
///     Represents a server-bound (client-sent) packet.
///     Most packets should implement <see cref="IServerBoundPacket{TSelf}" /> to also support reading capabilities.
/// </summary>
public interface IServerBoundPacket : IPacket;

/// <summary>
///     Represents a <see cref="IServerBoundPacket" /> also supporting reading via
///     <see cref="IReadableProtocolElement{TSelf}" />.
/// </summary>
public interface IServerBoundPacket<out TSelf> : IServerBoundPacket, IPacket<TSelf>;
