using System.Runtime.CompilerServices;

namespace Meetup.PDS.MurmurHash;

public static class SMurmurHash
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization)] 
    public static uint Hash(ReadOnlySpan<char> str, uint seed = 0x9747b28c)
    {
        const uint m = 0x5bd1e995;
        const int r = 24;

        var h = seed ^ (uint) str.Length;
        var len = str.Length;
        var currentIndex = 0;

        while (len >= 4)
        {
            var k = (uint) (str[currentIndex++] 
                            | str[currentIndex++] << 8 
                            | str[currentIndex++] << 16 
                            | str[currentIndex++] << 24);
            k *= m;
            k ^= k >> r;
            k *= m;

            h *= m;
            h ^= k;

            len -= 4;
        }

        if (len > 0)
        {
            uint k = 0;
            switch (len)
            {
                case 3:
                    k ^= (uint) str[currentIndex + 2] << 16;
                    goto case 2;
                case 2:
                    k ^= (uint) str[currentIndex + 1] << 8;
                    goto case 1;
                case 1:
                    k ^= str[currentIndex];
                    k *= m;
                    k ^= k >> r;
                    k *= m;
                    h ^= k;
                    break;
            }
        }

        h ^= (uint) str.Length;
        h ^= h >> 13;
        h *= m;
        h ^= h >> 15;

        return h;
    }

}