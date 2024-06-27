using InvestmentDistribution;
using System.Diagnostics;

List<Investment> investments =
[
    new Investment("Stocks", 0.06, 0.06),
    new Investment("Savings", 0.04, 0.02),
];

var assetAllocation = new AssetAllocation(investments);

var stopwatch = Stopwatch.StartNew();
var bestWeightings = assetAllocation.CalculateAllocations();
Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine();

Console.WriteLine(bestWeightings);