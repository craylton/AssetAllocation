namespace InvestmentDistribution;

internal class GradientDescent
{
    public int MaxIterations { get; init; } = 200;
    public double TargetYield { get; init; } = 0;

    public IEnumerable<WeightedInvestment> Optimise(Investment[] investments)
    {
        Random random = new Random();
        int numInvestments = investments.Length;
        double[] initialWeights = investments.Select(_ => 50d).ToArray();
        double[] bestWeights = initialWeights;
        double bestOutcome = 0d;
        double learningRate = 50d;

        int iterations = 0;
        while (iterations < MaxIterations)
        {
            var weightsArray = GetWeights(bestWeights, learningRate);
            learningRate *= 0.8;

            foreach (var weights in weightsArray)
            {
                CombinedInvestment uut = CombinedInvestment.From(investments, weights);
                double outcome = 1 - uut.Pdf.CumulativeDistribution(TargetYield);

                if (outcome > bestOutcome)
                {
                    bestWeights = weights;
                    bestOutcome = outcome;
                }
            }

            iterations++;
        }

        return investments.Select((investment, index) => WeightedInvestment.From(investment, bestWeights[index]));
    }

    private IEnumerable<double[]> GetWeights(double[] previousBest, double searchWidth)
    {
        double lower = Math.Clamp(previousBest[0] - searchWidth, 0.1, 100);
        double upper = Math.Clamp(previousBest[0] + searchWidth, 0.1, 100);
        if (previousBest.Length <= 1)
        {
            yield return [lower];
            yield return [upper];
        }
        else
        {
            IEnumerable<IEnumerable<double>> weightsList = GetWeights(previousBest[1..], searchWidth);

            foreach (var weights in weightsList)
            {
                yield return weights.Prepend(lower).ToArray();
                yield return weights.Prepend(upper).ToArray();
            }
        }
    }
}
