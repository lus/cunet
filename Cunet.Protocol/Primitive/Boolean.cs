using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct Boolean(bool value) : IProtocolElement<Boolean> {
    public bool Value { get; } = value;

    public int CalculateSize() {
        return 1;
    }

    public int Write(Span<byte> output) {
        output[0] = (byte)(Value ? 0x01 : 0x00);
        return 1;
    }

    public static Boolean Read(ReadOnlySpan<byte> input, out int consumed) {
        byte raw = input[0];

        consumed = 1;
        return raw switch {
            0x00 => new Boolean(false),
            0x01 => new Boolean(true),
            _ => throw new ReadException($"unexpected data (got {raw})"),
        };
    }

    public static implicit operator Boolean(bool value) {
        return new Boolean(value);
    }

    public static implicit operator bool(Boolean value) {
        return value.Value;
    }
}
