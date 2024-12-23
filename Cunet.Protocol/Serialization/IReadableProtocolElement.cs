using JetBrains.Annotations;

namespace Cunet.Protocol.Serialization;

/// <summary>
///     Represents a deserializable element of the Minecraft protocol.
/// </summary>
/// <typeparam name="TSelf">The type of the inheritor.</typeparam>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.WithMembers)]
public interface IReadableProtocolElement<out TSelf> {
    /// <summary>
    ///     Deserializes the protocol element from a byte <see cref="ReadOnlySpan{T}" />.
    /// </summary>
    /// <param name="input">The data source to read from.</param>
    /// <param name="consumed">An output parameter providing the amount of bytes read from the data source.</param>
    /// <returns>The deserialized protocol element.</returns>
    /// <exception cref="ReadException">If the element could not be read.</exception>
    public static abstract TSelf Read(ReadOnlySpan<byte> input, out int consumed);
}
