using InvestmentDistribution;
using System.Diagnostics;

List<Investment> investments =
[
    new Investment("Stocks", 1.06, 0.07),
    new Investment("Savings", 1.035, 0.01),
    new Investment("Bonds", 1.05, 0.07),
    new Investment("Property", 1.10, 0.10),
    new Investment("Cash under pillow", 0.995, 0.004),
    new Investment("Precious metals", 1.05, 0.06),
    new Investment("Crypto", 1.05, 0.25),
];

var assetAllocation = new AssetAllocation(investments);

var stopwatch = Stopwatch.StartNew();
var bestWeightings = assetAllocation.CalculateAllocations();
Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine();

Console.WriteLine(bestWeightings);