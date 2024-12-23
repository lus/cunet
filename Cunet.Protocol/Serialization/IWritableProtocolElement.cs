using JetBrains.Annotations;

namespace Cunet.Protocol.Serialization;

/// <summary>
///     Represents a serializable element of the Minecraft protocol.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.WithMembers)]
public interface IWritableProtocolElement {
    /// <summary>
    ///     Pre-calculates the amount of bytes expected to be produced when serializing the element.
    ///     This value has to be exact.
    /// </summary>
    /// <returns>
    ///     The amount of bytes expected to be produced and written by a call to <see cref="Write(System.Span{byte})" />
    /// </returns>
    public int CalculateSize();

    /// <summary>
    ///     Serializes and writes the data of the element to a byte <see cref="Span{T}" />.
    /// </summary>
    /// <param name="output">The <see cref="Span{T}" /> to write the data to.</param>
    /// <returns>The amount of bytes written.</returns>
    /// <exception cref="WriteException">If the element could not be written.</exception>
    public int Write(Span<byte> output);
}
