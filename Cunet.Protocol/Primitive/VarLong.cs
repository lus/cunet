using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct VarLong(long value) : IProtocolElement<VarLong> {
    public long Value { get; } = value;

    public int CalculateSize() {
        return WriteInternal(Span<byte>.Empty, true);
    }

    public int Write(Span<byte> output) {
        return WriteInternal(output, false);
    }

    private int WriteInternal(Span<byte> output, bool dryRun) {
        int offset = 0;

        long valueToWrite = Value;

        while (true) {
            if ((valueToWrite & ~(long)0x7F) == 0) {
                if (!dryRun) {
                    output[offset] = (byte)valueToWrite;
                }
                offset++;

                break;
            }

            if (!dryRun) {
                output[offset] = (byte)((valueToWrite & 0x7F) | 0x80);
            }
            offset++;

            valueToWrite >>>= 7;
        }

        return offset;
    }

    public static VarLong Read(ReadOnlySpan<byte> input, out int consumed) {
        int offset = 0;

        long readValue = 0;
        int offsetFromRight = 0;

        while (true) {
            byte readByte = input[offset++];

            // The 7 least significant bits store the value (mask = 0x7F)
            readValue |= (long)(readByte & 0x7F) << offsetFromRight;

            // The MSB stores whether another byte follows
            if ((readByte & 0x80) == 0) {
                break;
            }

            offsetFromRight += 7;
            if (offsetFromRight >= 64) {
                throw new ReadException("VarLong is too big");
            }
        }

        consumed = offset;
        return new VarLong(readValue);
    }

    public static implicit operator VarLong(long value) {
        return new VarLong(value);
    }

    public static implicit operator long(VarLong value) {
        return value.Value;
    }
}
