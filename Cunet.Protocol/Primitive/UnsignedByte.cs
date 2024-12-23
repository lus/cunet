using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct UnsignedByte(byte value) : IProtocolElement<UnsignedByte> {
    public byte Value { get; } = value;

    public int CalculateSize() {
        return 1;
    }

    public int Write(Span<byte> output) {
        output[0] = Value;
        return 1;
    }

    public static UnsignedByte Read(ReadOnlySpan<byte> input, out int consumed) {
        byte value = input[0];
        consumed = 1;
        return new UnsignedByte(value);
    }

    public static implicit operator UnsignedByte(byte value) {
        return new UnsignedByte(value);
    }

    public static implicit operator byte(UnsignedByte value) {
        return value.Value;
    }
}
