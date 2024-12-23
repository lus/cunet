using System.Buffers.Binary;
using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct Int(int value) : IProtocolElement<Int> {
    public int Value { get; } = value;

    public int CalculateSize() {
        return 4;
    }

    public int Write(Span<byte> output) {
        BinaryPrimitives.WriteInt32BigEndian(output, Value);
        return 4;
    }

    public static Int Read(ReadOnlySpan<byte> input, out int consumed) {
        int value = BinaryPrimitives.ReadInt32BigEndian(input);
        consumed = 4;
        return new Int(value);
    }

    public static implicit operator Int(int value) {
        return new Int(value);
    }

    public static implicit operator int(Int value) {
        return value.Value;
    }
}
