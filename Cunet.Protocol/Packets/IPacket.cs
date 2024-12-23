using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Packets;

/// <summary>
///     Represents a packet. This interface only ensures packet writing via <see cref="IWritableProtocolElement" />.
///     Most packets should implement <see cref="IPacket{TSelf}" /> which extends this interface with reading capabilities
///     via <see cref="IReadableProtocolElement{TSelf}" />.
/// </summary>
public interface IPacket : IWritableProtocolElement;

/// <summary>
///     Represents a <see cref="IPacket" /> supporting reading via <see cref="IReadableProtocolElement{TSelf}" />.
/// </summary>
/// <typeparam name="TSelf">The type of the inheritor.</typeparam>
public interface IPacket<out TSelf> : IPacket, IReadableProtocolElement<TSelf>;
