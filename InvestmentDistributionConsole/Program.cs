using InvestmentDistribution;
using System.Diagnostics;

List<Investment> investments =
[
    new Investment("Stocks", 0.06, 0.06),
    new Investment("Savings", 0.035, 0.02),
    new Investment("Bonds", 0.05, 0.06),
    new Investment("Property", 0.10, 0.08),
    new Investment("Cash under pillow", -0.005, 0.01),
    new Investment("Precious metals", 0.05, 0.05),
    new Investment("Crypto", 0.08, 0.2),
];

var assetAllocation = new AssetAllocation(investments);

var stopwatch = Stopwatch.StartNew();
var bestWeightings = assetAllocation.CalculateAllocations();
Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine();

Console.WriteLine(bestWeightings);