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

    internal double Simulate(double targetYield, int iterations, int numYears)
    {
        int counter = 0;

        int numSamples = iterations * numYears;
        double[][] samples = new double[_weightedInvestments.Count][];
        for (int i = 0; i < _weightedInvestments.Count; i++)
        {
            samples[i] = _weightedInvestments[i].Pdf.Samples().Take(numSamples).ToArray();
            for (int j = 0; j < samples[i].Length; j++)
            {
                samples[i][j] = (samples[i][j] - 1) * _weightedInvestments[i].Weight;
            }
        }

        for (int iterationIndex = 0; iterationIndex < numSamples; iterationIndex += numYears)
        {
            double yield = 1.00;
            for (int investmentIndex = 0; investmentIndex < _weightedInvestments.Count; investmentIndex++)
            {
                for (int yearIndex = 0; yearIndex < numYears; yearIndex++)
                {
                    yield += samples[investmentIndex][iterationIndex + yearIndex];
                }
            }

            if (yield > Math.Pow(targetYield, numYears))
                counter++;
        }

        return counter;
    }

    public IEnumerator<WeightedInvestment> GetEnumerator() => _weightedInvestments.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _weightedInvestments.GetEnumerator();
}
