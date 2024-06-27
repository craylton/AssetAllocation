using MathNet.Numerics.Distributions;

namespace InvestmentDistribution;

internal class CombinedInvestment(Normal pdf)
{
    public Normal Pdf { get; private set; } = pdf;

    public static CombinedInvestment From(IEnumerable<WeightedInvestment> investments)
    {
        double sumOfWeights = investments.Sum(x => x.Weight);
        double mean = CalculateCombinedMean(investments, sumOfWeights);
        double stdDev = CalculateCombinedStdDev(investments, sumOfWeights);
        return new CombinedInvestment(new Normal(mean, stdDev));
    }

    public static CombinedInvestment From(Investment[] investments, double[] weights)
    {
        var weightedInvestments = investments.Select(
            (investment, index) => WeightedInvestment.From(investment, weights[index]));

        return From(weightedInvestments);
    }

    private static double CalculateCombinedMean(IEnumerable<WeightedInvestment> investments, double sumOfWeights) =>
    investments.Sum(investments => investments.WeightedMean) / sumOfWeights;

    private static double CalculateCombinedStdDev(IEnumerable<WeightedInvestment> investments, double sumOfWeights) =>
        Math.Sqrt(investments.Sum(investments => investments.WeightedStdDev * investments.WeightedStdDev)) / sumOfWeights;
}
