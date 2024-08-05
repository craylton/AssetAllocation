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

InvestmentGoals investmentGoals = new(
    [
        new(1.04, 3),
        new(1.08, 1),
    ],
    2);

var assetAllocation = new AssetAllocation(investments, SimulationAccuracy.Normal);

var stopwatch = Stopwatch.StartNew();

var bestWeightings = assetAllocation.CalculateAllocations(investmentGoals);

foreach (var goal in investmentGoals)
{
    PrintChanceOfBeatingTarget(goal.Goal, investmentGoals.NumberOfYears, bestWeightings);
}

Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine();

Console.WriteLine(bestWeightings);

static void PrintChanceOfBeatingTarget(double target, int numberOfYears, WeightedInvestments bestWeightings)
{
    InvestmentGoals investmentGoals = new([new(target, 1)], numberOfYears);
    var successRate = bestWeightings.Simulate(investmentGoals, 5000) / 50;
    Console.WriteLine($"{successRate}% chance of getting more than {(target - 1) * 100:0.00}% annual return");
}