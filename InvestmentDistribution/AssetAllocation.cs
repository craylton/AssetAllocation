namespace InvestmentDistribution;

public class AssetAllocation(IEnumerable<Investment> investments, SimulationAccuracy simulationAccuracy)
{
    public IEnumerable<Investment> Investments { get; } = investments;
    public SimulationAccuracy SimulationAccuracy { get; } = simulationAccuracy;

    public WeightedInvestments CalculateAllocations(InvestmentGoals investmentGoals)
    {
        GradientDescent gradientDescent = new(SimulationAccuracy, investmentGoals);
        return gradientDescent.OptimiseWithSimulation(Investments.ToArray());
    }
}
