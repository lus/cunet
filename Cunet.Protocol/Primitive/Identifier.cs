using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct Identifier(string ns, string value) : IProtocolElement<Identifier> {
    private const string DefaultNamespace = "minecraft";

    private readonly String _wrapped = new(ns + ":" + value);

    public Identifier(string value) : this(DefaultNamespace, value) {
    }

    public string Key { get; } = ns;
    public string Value { get; } = value;

    public int CalculateSize() {
        return _wrapped.CalculateSize();
    }

    public int Write(Span<byte> output) {
        return _wrapped.Write(output);
    }

    public static Identifier Read(ReadOnlySpan<byte> input, out int consumed) {
        string raw = String.Read(input, out consumed).Value;
        if (string.IsNullOrEmpty(raw)) {
            throw new ReadException("identifier is empty");
        }

        string[] parts = raw.Split(':');
        return parts.Length switch {
            1 => new Identifier(parts[0]),
            2 => new Identifier(parts[0], parts[1]),
            _ => throw new ReadException("identifier is invalid"),
        };
    }
}
