namespace InvestmentDistribution;

internal class GradientDescent
{
    public int MaxIterations { get; init; } = 400;
    public double TargetYield { get; init; } = 1.03;

    public WeightedInvestments Optimise(Investment[] investments)
    {
        Random random = new Random();
        int numInvestments = investments.Length;
        Weights initialWeights = new Weights(investments.Select(_ => 50d).ToList());
        Weights bestWeights = initialWeights;
        double bestOutcome = 0d;
        double learningRate = 50d;

        int iterations = 0;
        while (iterations < MaxIterations / 5)
        {
            var weightsArray = GetWeightsToTest(bestWeights, learningRate);
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

    public WeightedInvestments OptimiseWithSimulation(Investment[] investments)
    {
        Random random = new Random();
        int numInvestments = investments.Length;
        Weights initialWeights = new Weights(Optimise(investments).Select(investments => investments.Weight).ToList());
        Weights bestWeights = initialWeights;
        double learningRate = 2d;

        int iterations = 0;
        while (iterations < MaxIterations)
        {
            bestWeights = initialWeights;
            double bestOutcome = 0d;
            var weightsArray = GetWeightsToTest(bestWeights, learningRate);
            learningRate *= 0.95;

            foreach (var weights in weightsArray.Select(weights => weights.Normalise()))
            {
                var weightedInvestments = WeightedInvestments.From(investments, weights);
                double outcome = weightedInvestments.Simulate(TargetYield, MaxIterations);

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

    private IEnumerable<Weights> GetWeightsToTest(Weights previousBest, double searchWidth)
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
            IEnumerable<Weights> weightsList = GetWeightsToTest(reducedWeights, searchWidth);

            foreach (var weights in weightsList)
            {
                yield return weights.Prepend(lower);
                yield return weights.Prepend(upper);
            }
        }
    }
}
