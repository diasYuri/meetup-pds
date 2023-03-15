using Meetup.PDS.BF;

var bloomFilter = new BloomFilter(93, 13);
var items = new []
{
    "laranja",
    "maca",
    "limao"
};

foreach (var item in items)
{
    bloomFilter.Add(item);
}

Console.WriteLine(bloomFilter.MaybeContains("laranja"));
Console.WriteLine(bloomFilter.MaybeContains("pessego"));