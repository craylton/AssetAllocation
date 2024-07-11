namespace InvestmentDistribution;

public class AssetAllocation(IEnumerable<Investment> investments)
{
    public IEnumerable<Investment> Investments { get; } = investments;

    public WeightedInvestments CalculateAllocations(double targetYield, int numYears)
    {
        GradientDescent gradientDescent = new(1e-5, 100, targetYield, numYears);
        return gradientDescent.OptimiseWithSimulation(Investments.ToArray());
    }
}
