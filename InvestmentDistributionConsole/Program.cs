using InvestmentDistribution;
using System.Diagnostics;

List<Investment> investments =
[
    new Investment(0.06, 0.06),
    new Investment(0.04, 0.02),
];

AssetAllocation assetAllocation = new AssetAllocation(investments);

var stopwatch = Stopwatch.StartNew();
var bestWeightings = assetAllocation.CalculateAllocations();
Console.WriteLine(stopwatch.ElapsedMilliseconds);
stopwatch.Stop();

foreach (var weightedInvestment in bestWeightings)
{
    Console.WriteLine(weightedInvestment);
}