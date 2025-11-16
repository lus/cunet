using System.Buffers.Binary;
using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct Float(float value) : IProtocolElement<Float> {
    public float Value { get; } = value;

    public int CalculateSize() {
        return 4;
    }

    public int Write(Span<byte> output) {
        BinaryPrimitives.WriteSingleBigEndian(output, Value);
        return 4;
    }

    public static Float Read(ReadOnlySpan<byte> input, out int consumed) {
        float value = BinaryPrimitives.ReadSingleBigEndian(input);
        consumed = 4;

        return new Float(value);
    }

    public static implicit operator Float(float value) {
        return new Float(value);
    }

    public static implicit operator float(Float value) {
        return value.Value;
    }
}
