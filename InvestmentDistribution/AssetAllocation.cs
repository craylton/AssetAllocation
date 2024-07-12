namespace InvestmentDistribution;

public class AssetAllocation(IEnumerable<Investment> investments, SimulationAccuracy simulationAccuracy)
{
    public IEnumerable<Investment> Investments { get; } = investments;
    public SimulationAccuracy SimulationAccuracy { get; } = simulationAccuracy;

    public WeightedInvestments CalculateAllocations(double targetYield, int numYears)
    {
        GradientDescent gradientDescent = new(SimulationAccuracy, targetYield, numYears);
        return gradientDescent.OptimiseWithSimulation(Investments.ToArray());
    }
}
