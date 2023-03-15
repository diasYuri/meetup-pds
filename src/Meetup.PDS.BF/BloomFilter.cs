using System.Runtime.CompilerServices;

namespace Meetup.PDS.BF;

public class BloomFilter
{
    private readonly byte[] _filter;
    private readonly int _filterSize;
    private readonly int _numHashFunctions;

    public BloomFilter(int bitSize, int numHashFunctions)
    {
        _filter = new byte[bitSize/8];
        _filterSize = bitSize;
        _numHashFunctions = numHashFunctions;
    }

    public void Add(ReadOnlySpan<char> str)
    {
        for (uint i = 0; i < _numHashFunctions; i++)
        {
            var (byteIndex, bitIndex) = GetIndex(str, i);
            _filter[byteIndex] |= (byte) (1 << bitIndex);
        }
    }

    public bool MaybeContains(ReadOnlySpan<char> str)
    {
        for (uint i = 0; i < _numHashFunctions; i++)
        {
            var (byteIndex, bitIndex) = GetIndex(str, i);
            if ((_filter[byteIndex] & (1 << bitIndex)) == 0)
            {
                return false;
            }
        }
        return true;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private (int byteIndex, int bitIndex) GetIndex(ReadOnlySpan<char> str, uint i)
    {
        var hash = MurmurHash.SMurmurHash.Hash(str, i);
        var index = (int) (hash % (uint) _filterSize);
        var byteIndex = index / 8;
        var bitIndex = index % 8;
        return (byteIndex, bitIndex);
    }
    
}