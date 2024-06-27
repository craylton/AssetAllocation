namespace InvestmentDistribution;

public class AssetAllocation(IEnumerable<Investment> investments)
{
    public IEnumerable<Investment> Investments { get; } = investments;

    public IEnumerable<WeightedInvestment> CalculateAllocations()
    {
        GradientDescent gradientDescent = new();
        return gradientDescent.Optimise(Investments.ToArray());
    }
}
