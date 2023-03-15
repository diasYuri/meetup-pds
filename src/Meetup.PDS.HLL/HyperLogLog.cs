namespace Meetup.PDS.HLL;

public class HyperLogLog
{
    private readonly uint[] _buckets;
    private readonly int _p;
    private readonly double _alphaMm;

    public HyperLogLog(int p)
    {
        _p = p;
        var m = 1 << p;
        _buckets = new uint[m];
        _alphaMm = GetAlpha(m);
    }

    private static double GetAlpha(int m)
    {
        var alpha = 0.7213 / (1 + 1.079 / m);
        return alpha * m * m;
    }

    public void Add(ReadOnlySpan<char> str)
    {
        var hash = MurmurHash.SMurmurHash.Hash(str);
        var index = hash >> (32 - _p);
        var rank = hash << _p;
        rank >>= _p + 1;
        rank |= (uint)(1 << _p);
        var oldVal = _buckets[index];
        _buckets[index] = Math.Max(rank, oldVal);
    }

    public int Count()
    {
        var est = Calculate();
        
        if (est <= 2.5 * _buckets.Length)
        {
            var zeros = _buckets.Count(r => r == 0);
            return (int) (_buckets.Length * Math.Log((double) _buckets.Length / zeros));
        }
        if (est <= (1 / 30.0) * Math.Pow(2, 32))
        {
            return (int) est;
        }
        return (int) (-Math.Pow(2, 32) * Math.Log(1 - est / Math.Pow(2, 32)));
    }

    private double Calculate()
    {
        var sum = _buckets.Sum(bucket => 1.0 / (1 << (int)bucket));
        var estimate = _alphaMm / sum;
        return estimate;
    }
}