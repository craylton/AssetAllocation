using System.Collections.Concurrent;

namespace InvestmentDistribution;

internal class GradientDescent
{
    public double Threshold { get; init; } = 1e-5;
    public int SimulationSize { get; set; } = 100;
    public double TargetYield { get; init; } = 1.03;

    public WeightedInvestments OptimiseWithSimulation(Investment[] investments)
    {
        int numInvestments = investments.Length;
        Weights initialWeights = new Weights(investments.Select(_ => 1d).ToList()).Normalise();
        Weights bestWeights = initialWeights;
        double learningRate = 0.16d;

        int iterations = 0;
        while (learningRate > 0.00001)
        {
            ConcurrentDictionary<Weights, double> results = [];
            var weightsArray = GetWeightsToTest(bestWeights, learningRate);
            learningRate *= 0.975;

            Parallel.ForEach(weightsArray.Select(weights => weights.Normalise()), weights =>
            {
                var weightedInvestments = WeightedInvestments.From(investments, weights);
                double outcome = weightedInvestments.Simulate(TargetYield, SimulationSize, 5);
                results[weights] = outcome;
            });

            bestWeights = new Weights(GetNewWeightsFromResults(investments.Length, results));
            iterations++;
        }

        var a = WeightedInvestments.From(investments, bestWeights);
        var successRate = a.Simulate(TargetYield, SimulationSize * 100, 5) / (SimulationSize * 100);
        Console.WriteLine($"{successRate * 100}% chance of reaching target");
        return a;
    }

    private static double[] GetNewWeightsFromResults(int numInvestments, ConcurrentDictionary<Weights, double> results)
    {
        List<KeyValuePair<Weights, double>> resultsList = SortResults(results);
        Weights[] bestWeightsLists = resultsList
            .Take(Math.Max(results.Count / 10, 1))
            .Select(res => res.Key)
            .ToArray();

        double[] newWeights = new double[numInvestments];
        for (int i = 0; i < numInvestments; i++)
        {
            for (int j = 0; j < bestWeightsLists.Length; j++)
            {
                newWeights[i] += bestWeightsLists[j][i];
            }
            newWeights[i] /= bestWeightsLists.Length;
        }

        return newWeights;
    }

    private static List<KeyValuePair<Weights, double>> SortResults(ConcurrentDictionary<Weights, double> results)
    {
        List<KeyValuePair<Weights, double>> resultsList = [.. results];
        resultsList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
        return resultsList;
    }

    private IEnumerable<Weights> GetWeightsToTest(Weights previousBest, double searchWidth)
    {
        double lower = Math.Clamp(previousBest[0] - searchWidth, 0.0000001, 1);
        double upper = Math.Clamp(previousBest[0] + searchWidth, 0.0000001, 1);

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
