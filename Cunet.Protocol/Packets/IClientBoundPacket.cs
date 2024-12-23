using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Packets;

/// <summary>
///     Represents a client-bound (server-sent) packet.
///     Most packets should implement <see cref="IClientBoundPacket{TSelf}" /> to also support reading capabilities.
/// </summary>
public interface IClientBoundPacket : IPacket;

/// <summary>
///     Represents a <see cref="IClientBoundPacket" /> also supporting reading via
///     <see cref="IReadableProtocolElement{TSelf}" />.
/// </summary>
public interface IClientBoundPacket<out TSelf> : IClientBoundPacket, IPacket<TSelf>;
