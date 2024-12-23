using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct Byte(sbyte value) : IProtocolElement<Byte> {
    public sbyte Value { get; } = value;

    public int CalculateSize() {
        return 1;
    }

    public int Write(Span<byte> output) {
        output[0] = (byte)Value;
        return 1;
    }

    public static Byte Read(ReadOnlySpan<byte> input, out int consumed) {
        byte value = input[0];
        consumed = 1;
        return new Byte((sbyte)value);
    }
}
