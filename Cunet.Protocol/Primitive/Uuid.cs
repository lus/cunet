using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct Uuid(Guid value) : IProtocolElement<Uuid> {
    public Guid Value { get; } = value;

    public int CalculateSize() {
        return 16;
    }

    public int Write(Span<byte> buffer) {
        byte[] bytes = Value.ToByteArray(true);
        bytes.CopyTo(buffer);
        return 16;
    }

    public static Uuid Read(ReadOnlySpan<byte> input, out int consumed) {
        Guid value = new(input[..16], true);
        consumed = 16;
        return new Uuid(value);
    }

    public static implicit operator Uuid(Guid value) {
        return new Uuid(value);
    }

    public static implicit operator Guid(Uuid value) {
        return value.Value;
    }
}
