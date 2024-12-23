namespace Cunet.Protocol.Serialization;

/// <summary>
///     Represents an element of the Minecraft protocol that can be both read and written. It is a composite of
///     <see cref="IWritableProtocolElement" /> and <see cref="IReadableProtocolElement{TSelf}" />.
/// </summary>
/// <typeparam name="TSelf">The type of the inheritor.</typeparam>
public interface IProtocolElement<out TSelf> : IWritableProtocolElement, IReadableProtocolElement<TSelf>;
