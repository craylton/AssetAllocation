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
        double overallTargetYield = Math.Pow(targetYield, numYears);
        double[][] samples = GetSamples(iterations, numYears);

        for (int iterationIndex = 0; iterationIndex < iterations; iterationIndex++)
        {
            double yield = 1.00;
            for (int investmentIndex = 0; investmentIndex < _weightedInvestments.Count; investmentIndex++)
            {
                double weight = _weightedInvestments[investmentIndex].Weight;
                for (int yearIndex = 0; yearIndex < numYears; yearIndex++)
                {
                    yield += (samples[investmentIndex][iterationIndex * numYears + yearIndex] - 1) * weight;
                }
            }

            if (yield > overallTargetYield)
                counter++;
        }

        return counter;
    }

    private double[][] GetSamples(int iterations, int numYears)
    {
        int numSamples = iterations * numYears;
        double[][] samples = new double[_weightedInvestments.Count][];

        for (int i = 0; i < _weightedInvestments.Count; i++)
        {
            samples[i] = new double[numSamples];
            _weightedInvestments[i].Pdf.Samples(samples[i]);
        }

        return samples;
    }

    public IEnumerator<WeightedInvestment> GetEnumerator() => _weightedInvestments.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _weightedInvestments.GetEnumerator();
}
