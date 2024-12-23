using JetBrains.Annotations;

namespace Cunet.Protocol.Serialization;

/// <summary>
///     Helps keeping track of the offset and amount of consumed bytes when reading
///     <see cref="IReadableProtocolElement{TSelf}" />s.
/// </summary>
/// <param name="data">The data source to read from.</param>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public ref struct ProtocolReader(ReadOnlySpan<byte> data) {
    private ReadOnlySpan<byte> _data = data;

    /// <summary>
    ///     The total amount of consumed bytes.
    /// </summary>
    public int TotalConsumed { get; private set; } = 0;

    /// <summary>
    ///     Reads a <see cref="IReadableProtocolElement{TSelf}" /> from the given input <see cref="data" /> and increases the
    ///     <see cref="TotalConsumed" /> counter by the element size.
    /// </summary>
    /// <typeparam name="T">The type of the element to read.</typeparam>
    /// <returns>The read element.</returns>
    public T Read<T>() where T : IReadableProtocolElement<T> {
        T read = T.Read(_data[TotalConsumed..], out int consumed);
        TotalConsumed += consumed;
        return read;
    }
}
