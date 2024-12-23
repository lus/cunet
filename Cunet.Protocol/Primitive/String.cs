using System.Text;
using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct String(string value) : IProtocolElement<String> {
    public string Value { get; } = value;

    public int CalculateSize() {
        int utf8Bytes = Encoding.UTF8.GetByteCount(Value);
        return utf8Bytes + new VarInt(utf8Bytes).CalculateSize();
    }

    public int Write(Span<byte> output) {
        byte[] bytesToWrite = Encoding.UTF8.GetBytes(Value);
        int written = new VarInt(bytesToWrite.Length).Write(output);
        bytesToWrite.CopyTo(output[written..]);
        return written + bytesToWrite.Length;
    }

    public static String Read(ReadOnlySpan<byte> input, out int consumed) {
        int size = VarInt.Read(input, out int consumedBySize).Value;
        string value = Encoding.UTF8.GetString(input.Slice(consumedBySize, size));
        consumed = consumedBySize + size;
        return new String(value);
    }
}
