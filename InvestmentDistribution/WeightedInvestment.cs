namespace InvestmentDistribution;

public class WeightedInvestment(double mean, double standardDeviation, double weight)
    : Investment(mean, standardDeviation)
{
    public double Weight { get; private set; } = weight;
    public double WeightedMean => Weight * Mean;
    public double WeightedStdDev => Weight * StandardDeviation;

    public static WeightedInvestment From(Investment investment, double weight) =>
        new WeightedInvestment(investment.Mean, investment.StandardDeviation, weight);

    public override string ToString() => Weight.ToString();
}