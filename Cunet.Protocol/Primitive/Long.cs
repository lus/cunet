using System.Buffers.Binary;
using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct Long(long value) : IProtocolElement<Long> {
    public long Value { get; } = value;

    public int CalculateSize() {
        return 8;
    }

    public int Write(Span<byte> output) {
        BinaryPrimitives.WriteInt64BigEndian(output, Value);
        return 8;
    }

    public static Long Read(ReadOnlySpan<byte> input, out int consumed) {
        long value = BinaryPrimitives.ReadInt64BigEndian(input);
        consumed = 8;
        return new Long(value);
    }

    public static implicit operator Long(long value) {
        return new Long(value);
    }

    public static implicit operator long(Long value) {
        return value.Value;
    }
}
