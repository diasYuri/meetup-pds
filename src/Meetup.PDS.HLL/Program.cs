using Meetup.PDS.HLL;

var hll = new HyperLogLog(12);

hll.Add("laranja");
hll.Add("maça");
hll.Add("maça");
hll.Add("maça");
hll.Add("maça");
hll.Add("maça");
hll.Add("maça");
hll.Add("maça");
hll.Add("laranja");
hll.Add("limão");

var count = hll.Count(); 
Console.WriteLine(count);