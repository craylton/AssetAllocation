namespace InvestmentDistribution;

public class AssetAllocation(IEnumerable<Investment> investments)
{
    public IEnumerable<Investment> Investments { get; } = investments;

    public WeightedInvestments CalculateAllocations()
    {
        GradientDescent gradientDescent = new();
        return gradientDescent.Optimise(Investments.ToArray());
    }
}
