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

    public double Simulate(InvestmentGoals investmentGoals, int iterations)
    {
        double score = 0;
        int numYears = investmentGoals.NumberOfYears;
        double[][] samples = GetSamples(iterations, numYears);

        for (int iterationIndex = 0; iterationIndex < iterations; iterationIndex++)
        {
            double yield = 1.00;
            for (int yearIndex = 0; yearIndex < numYears; yearIndex++)
            {
                yield *= samples[yearIndex][iterationIndex];
            }

            foreach (var goal in investmentGoals)
            {
                if (yield > Math.Pow(goal.Goal, numYears))
                    score += goal.Importance;
            }
        }

        return score;
    }

    private double[][] GetSamples(int iterations, int numYears)
    {
        CombinedInvestment combinedInvestment = CombinedInvestment.From(this);
        double[][] samples = new double[numYears][];

        for (int i = 0; i < numYears; i++)
        {
            samples[i] = new double[iterations];
            combinedInvestment.Pdf.Samples(samples[i]);
        }

        return samples;
    }

    public IEnumerator<WeightedInvestment> GetEnumerator() => _weightedInvestments.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _weightedInvestments.GetEnumerator();
}
