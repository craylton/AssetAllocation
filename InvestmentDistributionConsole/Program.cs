using InvestmentDistribution;
using System.Diagnostics;

List<Investment> investments =
[
    new Investment("Stocks", 1.06, 0.07),
    new Investment("Savings", 1.035, 0.01),
    new Investment("Bonds", 1.05, 0.07),
    new Investment("Property", 1.08, 0.11),
    new Investment("Cash under pillow", 0.995, 0.004),
    new Investment("Precious metals", 1.05, 0.06),
    new Investment("Crypto", 1.05, 0.25),
];

var assetAllocation = new AssetAllocation(investments, SimulationAccuracy.Normal);
double target = 1.05;

var stopwatch = Stopwatch.StartNew();

var bestWeightings = assetAllocation.CalculateAllocations(target, 2);

PrintChanceOfBeatingTarget(target, bestWeightings, "target");
PrintChanceOfBeatingTarget(1.02, bestWeightings, "inflation");
PrintChanceOfBeatingTarget(1.06, bestWeightings, "global stocks");

Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine();

Console.WriteLine(bestWeightings);

static void PrintChanceOfBeatingTarget(double target, WeightedInvestments bestWeightings, string targetName)
{
    var successRate = bestWeightings.Simulate(target, 5000, 5) / 50;
    Console.WriteLine($"{successRate}% chance of beating {targetName} ({(target - 1) * 100:0.00}%)");
}