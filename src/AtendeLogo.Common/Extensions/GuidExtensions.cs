using System.Runtime.InteropServices;

namespace AtendeLogo.Common.Extensions;

public static class GuidExtensions
{
    public static bool IsZeroPrefixedGuid(this Guid guid)
    {
        var zero = (byte)0x0;

        var bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref guid, 1));
        return bytes[0] == zero &&
               bytes[1] == zero &&
               bytes[2] == zero &&
               bytes[3] == zero &&
               bytes[4] == zero;
    }

    public static Guid NewGuidZeroPrefixed()
    {
        var guid = Guid.NewGuid();
        var zero = (byte)0x0;
        var bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref guid, 1));
        bytes[0] = zero;
        bytes[1] = zero;
        bytes[2] = zero;
        bytes[3] = zero;
        bytes[4] = zero;
        return MemoryMarshal.Read<Guid>(bytes);
    }
}
