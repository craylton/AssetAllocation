using System.Collections;
using System.Text;

namespace InvestmentDistribution;

public class WeightedInvestments(List<WeightedInvestment> weightedInvestments) : IEnumerable<WeightedInvestment>
{
    private readonly List<WeightedInvestment> _weightedInvestments = weightedInvestments;

    public static WeightedInvestments From(IEnumerable<Investment> investments, double[] weights)
    {
        double sumOfWeights = weights.Sum();
        double[] normalisedWeights = weights.Select(weight => weight * 100 / sumOfWeights).ToArray();

        var weightedInvestments = investments.Select(
            (investment, index) => WeightedInvestment.From(investment, normalisedWeights[index]));

        return new WeightedInvestments(weightedInvestments.ToList());
    }

    public double CalculateCombinedMean() =>
        _weightedInvestments.Sum(investments => investments.WeightedMean) / CalculateSumOfWeights();

    public double CalculateCombinedStdDev()
    {
        var sumOfSquares = _weightedInvestments.Sum(investments => investments.WeightedStdDev * investments.WeightedStdDev);
        return Math.Sqrt(sumOfSquares) / CalculateSumOfWeights();
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach (var weightedInvestment in _weightedInvestments)
        {
            stringBuilder.Append(weightedInvestment);
            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }

    private double CalculateSumOfWeights() => _weightedInvestments.Sum(x => x.Weight);
    public IEnumerator<WeightedInvestment> GetEnumerator() => _weightedInvestments.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _weightedInvestments.GetEnumerator();
}
