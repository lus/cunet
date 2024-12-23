using System.Buffers.Binary;
using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct Short(short value) : IProtocolElement<Short> {
    public short Value { get; } = value;

    public int CalculateSize() {
        return 2;
    }

    public int Write(Span<byte> output) {
        BinaryPrimitives.WriteInt16BigEndian(output, Value);
        return 2;
    }

    public static Short Read(ReadOnlySpan<byte> input, out int consumed) {
        short value = BinaryPrimitives.ReadInt16BigEndian(input);
        consumed = 2;
        return new Short(value);
    }
}
