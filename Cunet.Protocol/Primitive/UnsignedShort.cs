using System.Buffers.Binary;
using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct UnsignedShort(ushort value) : IProtocolElement<UnsignedShort> {
    public ushort Value { get; } = value;

    public int CalculateSize() {
        return 2;
    }

    public int Write(Span<byte> output) {
        BinaryPrimitives.WriteUInt16BigEndian(output, Value);
        return 2;
    }

    public static UnsignedShort Read(ReadOnlySpan<byte> input, out int consumed) {
        ushort value = BinaryPrimitives.ReadUInt16BigEndian(input);
        consumed = 2;
        return new UnsignedShort(value);
    }

    public static implicit operator UnsignedShort(ushort value) {
        return new UnsignedShort(value);
    }

    public static implicit operator ushort(UnsignedShort value) {
        return value.Value;
    }
}
