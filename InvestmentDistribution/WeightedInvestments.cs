using System.Collections;
using System.Text;

namespace InvestmentDistribution;

public class WeightedInvestments(IList<WeightedInvestment> weightedInvestments) : IEnumerable<WeightedInvestment>
{
    private readonly IList<WeightedInvestment> _weightedInvestments = weightedInvestments;

    public static WeightedInvestments From(IEnumerable<Investment> investments, Weights weights)
    {
        List<WeightedInvestment> weightedInvestments = investments
            .Select((investment, index) => WeightedInvestment.From(investment, weights[index]))
            .ToList();

        return new WeightedInvestments(weightedInvestments);
    }

    public double CalculateCombinedMean() =>
        _weightedInvestments.Sum(investment => investment.WeightedMean);

    public double CalculateCombinedStdDev()
    {
        var sumOfSquares = _weightedInvestments.Sum(investment => investment.WeightedStdDev * investment.WeightedStdDev);
        return Math.Sqrt(sumOfSquares);
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach (WeightedInvestment weightedInvestment in _weightedInvestments)
        {
            stringBuilder.Append(weightedInvestment);
            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }

    internal double Simulate(double targetYield, double iterations)
    {
        int counter = 0;
        for (int i = 0; i < iterations; i++)
        {
            double yield = 1.00;
            foreach (var investment in _weightedInvestments)
            {
                var sample = investment.Pdf.Sample();
                yield *= ((sample - 1) * investment.Weight) + 1;
            }

            if (yield > targetYield)
                counter++;
        }
        return counter;
    }

    public IEnumerator<WeightedInvestment> GetEnumerator() => _weightedInvestments.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _weightedInvestments.GetEnumerator();
}
