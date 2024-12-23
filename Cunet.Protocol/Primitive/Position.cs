using Cunet.Protocol.Serialization;

namespace Cunet.Protocol.Primitive;

public readonly struct Position(int x, int y, int z) : IProtocolElement<Position> {
    private readonly Long _wrapped =
        new((((long)x & 0x3FFFFFF) << 38) | (((long)z & 0x3FFFFFF) << 12) | ((long)y & 0xFFF));

    public int X { get; } = x;
    public int Y { get; } = y;
    public int Z { get; } = z;

    public int CalculateSize() {
        return _wrapped.CalculateSize();
    }

    public int Write(Span<byte> output) {
        return _wrapped.Write(output);
    }

    public static Position Read(ReadOnlySpan<byte> input, out int consumed) {
        long raw = Long.Read(input, out consumed).Value;

        int rawX = (int)(raw >> 38);
        int rawZ = (int)((raw << 26) >> 38);
        int rawY = (int)((raw << 52) >> 52);

        return new Position(rawX, rawY, rawZ);
    }
}
