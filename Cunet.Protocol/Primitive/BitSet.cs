using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct BitSet(int length, long[] data) : IProtocolElement<BitSet> {
    public int Length { get; } = length;
    public long[] Data { get; } = data;

    public int CalculateSize() {
        // A Longs size is exactly 8 bytes.
        return new VarInt(Data.Length).CalculateSize() + Data.Length * 8;
    }

    public int Write(Span<byte> output) {
        if (Length > Data.Length) {
            throw new WriteException("indicated BitSet length is bigger than data length");
        }

        ProtocolWriter writer = new(output);
        writer.Write(new VarInt(Length));
        foreach (long raw in Data) {
            writer.Write(new Long(raw));
        }
        return writer.TotalWritten;
    }

    public static BitSet Read(ReadOnlySpan<byte> input, out int consumed) {
        ProtocolReader reader = new(input);

        int length = reader.Read<VarInt>().Value;
        long[] data = new long[length];
        for (int i = 0; i < length; i++) {
            data[i] = reader.Read<Long>().Value;
        }

        consumed = reader.TotalConsumed;
        return new BitSet(length, data);
    }
}
