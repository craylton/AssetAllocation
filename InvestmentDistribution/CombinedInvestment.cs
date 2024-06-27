using MathNet.Numerics.Distributions;

namespace InvestmentDistribution;

internal class CombinedInvestment(Normal pdf)
{
    public Normal Pdf { get; private set; } = pdf;

    public static CombinedInvestment From(WeightedInvestments investments)
    {
        var pdf = new Normal(investments.CalculateCombinedMean(), investments.CalculateCombinedStdDev());
        return new CombinedInvestment(pdf);
    }

    public static CombinedInvestment From(Investment[] investments, double[] weights) =>
        From(WeightedInvestments.From(investments, weights));
}
