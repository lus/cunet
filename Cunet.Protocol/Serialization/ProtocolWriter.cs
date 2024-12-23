using JetBrains.Annotations;

namespace Cunet.Protocol.Serialization;

/// <summary>
///     Helps keeping track of the offset when writing <see cref="IWritableProtocolElement" />s.
/// </summary>
/// <param name="data">The data source to write to.</param>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public ref struct ProtocolWriter(Span<byte> data) {
    private Span<byte> _data = data;

    /// <summary>
    ///     The total amount of written bytes.
    /// </summary>
    public int TotalWritten { get; private set; } = 0;

    /// <summary>
    ///     Writes a <see cref="IWritableProtocolElement" /> to the given <see cref="data" /> and increases the
    ///     <see cref="TotalWritten" /> counter by the element size.
    /// </summary>
    /// <typeparam name="T">The type of the element to write.</typeparam>
    /// <returns>This writer to allow chaining.</returns>
    public ProtocolWriter Write<T>(T element) where T : IWritableProtocolElement {
        TotalWritten += element.Write(_data[TotalWritten..]);
        return this;
    }
}
