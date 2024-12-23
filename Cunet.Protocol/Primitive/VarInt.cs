using System.Buffers;
using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct VarInt(int value) : IProtocolElement<VarInt> {
    public int Value { get; } = value;

    public int CalculateSize() {
        return WriteInternal(Span<byte>.Empty, true);
    }

    public int Write(Span<byte> output) {
        return WriteInternal(output, false);
    }

    private int WriteInternal(Span<byte> output, bool dryRun) {
        int offset = 0;
        int valueToWrite = Value;

        while (true) {
            if ((valueToWrite & ~0x7F) == 0) {
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

    public static VarInt Read(ReadOnlySpan<byte> input, out int consumed) {
        int offset = 0;

        int readValue = 0;
        int offsetFromRight = 0;

        while (true) {
            byte readByte = input[offset++];

            // The 7 least significant bits store the value (mask = 0x7F)
            readValue |= (readByte & 0x7F) << offsetFromRight;

            // The MSB stores whether another byte follows
            if ((readByte & 0x80) == 0) {
                break;
            }

            offsetFromRight += 7;
            if (offsetFromRight >= 32) {
                throw new ReadException("VarInt is too big");
            }
        }

        consumed = offset;
        return new VarInt(readValue);
    }

    /// <summary>
    ///     Tries to read a VarInt from a <see cref="ReadOnlySequence{T}" />.
    ///     This is the only type that implements this functionality by default because it is used to prefix received packet
    ///     data by its length. As we do not know the length of a VarInt without knowing its value, we also do not know if we
    ///     have received enough bytes over the network to read the length prefix. If we in fact have received enough data
    ///     (i.e. this function returns <c>true</c>), the decoded size can then be used to make sure that we have received the
    ///     whole packet, rendering a <c>TryRead</c> equivalent for other packet elements unnecessary.
    /// </summary>
    /// <param name="input">The data source to read from.</param>
    /// <param name="value">The resulting value if this function returns <c>true</c>, otherwise <c>null</c>.</param>
    /// <param name="consumed">The amount of consumed bytes. <c>0</c> if the value could not be read.</param>
    /// <returns>Whether the function succeeded.</returns>
    /// <exception cref="ReadException">If the received VarInt is too long.</exception>
    public static bool TryRead(
        ReadOnlySequence<byte> input,
        out VarInt? value,
        out int consumed
    ) {
        SequenceReader<byte> reader = new(input);

        int readValue = 0;
        int offsetFromRight = 0;

        while (true) {
            if (!reader.TryRead(out byte readByte)) {
                value = null;
                consumed = 0;
                return false;
            }

            // The 7 least significant bits store the value (mask = 0x7F)
            readValue |= (readByte & 0x7F) << offsetFromRight;

            // The MSB stores whether another byte follows
            if ((readByte & 0x80) == 0) {
                break;
            }

            offsetFromRight += 7;
            if (offsetFromRight >= 32) {
                throw new ReadException("VarInt is too big");
            }
        }

        value = new VarInt(readValue);
        consumed = (int)reader.Consumed;
        return true;
    }

    public static implicit operator VarInt(int value) {
        return new VarInt(value);
    }

    public static implicit operator int(VarInt value) {
        return value.Value;
    }
}
