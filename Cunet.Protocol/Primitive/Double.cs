using System.Buffers.Binary;
using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct Double(double value) : IProtocolElement<Double> {
    public double Value { get; } = value;

    public int CalculateSize() {
        return 8;
    }

    public int Write(Span<byte> output) {
        BinaryPrimitives.WriteDoubleBigEndian(output, Value);
        return 8;
    }

    public static Double Read(ReadOnlySpan<byte> input, out int consumed) {
        double value = BinaryPrimitives.ReadDoubleBigEndian(input);
        consumed = 8;
        return new Double(value);
    }
}
