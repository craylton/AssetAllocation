namespace InvestmentDistribution;

internal class GradientDescent
{
    public int MaxIterations { get; init; } = 50;
    public double TargetYield { get; init; } = 0;

    public WeightedInvestments Optimise(Investment[] investments)
    {
        Random random = new Random();
        int numInvestments = investments.Length;
        Weights initialWeights = new Weights(investments.Select(_ => 50d).ToList());
        Weights bestWeights = initialWeights;
        double bestOutcome = 0d;
        double learningRate = 50d;

        int iterations = 0;
        while (iterations < MaxIterations)
        {
            var weightsArray = GetWeights(bestWeights, learningRate);
            learningRate *= 0.7 + random.NextDouble() * 0.1;

            foreach (var weights in weightsArray.Select(weights => weights.Normalise()))
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

        return WeightedInvestments.From(investments, bestWeights);
    }

    private IEnumerable<Weights> GetWeights(Weights previousBest, double searchWidth)
    {
        double lower = Math.Clamp(previousBest[0] - searchWidth, 0.00001, 100);
        double upper = Math.Clamp(previousBest[0] + searchWidth, 0.00001, 100);

        if (previousBest.Count <= 1)
        {
            yield return new Weights([lower]);
            yield return new Weights([upper]);
        }
        else
        {
            var reducedWeights = new Weights(previousBest.ToArray()[1..].ToList());
            IEnumerable<Weights> weightsList = GetWeights(reducedWeights, searchWidth);

            foreach (var weights in weightsList)
            {
                yield return weights.Prepend(lower);
                yield return weights.Prepend(upper);
            }
        }
    }
}
