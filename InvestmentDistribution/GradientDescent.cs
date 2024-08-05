using System.Collections.Concurrent;

namespace InvestmentDistribution;

internal class GradientDescent(SimulationAccuracy simulationAccuracy, InvestmentGoals investmentGoals)
{
    public double Threshold { get; } = simulationAccuracy.Threshold;
    public int SimulationSize { get; } = simulationAccuracy.SamplesPerSimulation;
    public InvestmentGoals InvestmentGoals { get; } = investmentGoals;

    public WeightedInvestments OptimiseWithSimulation(Investment[] investments)
    {
        int numInvestments = investments.Length;
        Weights initialWeights = new Weights(investments.Select(_ => 1d).ToList()).Normalise();
        Weights bestWeights = initialWeights;
        double learningRate = 0.16d;

        int iterations = 0;
        while (learningRate > Threshold)
        {
            ConcurrentDictionary<Weights, double> results = [];
            var weightsArray = GetWeightsToTest(bestWeights, learningRate);
            learningRate *= 0.975;

            Parallel.ForEach(weightsArray.Select(weights => weights.Normalise()), weights =>
            {
                var weightedInvestments = WeightedInvestments.From(investments, weights);
                double outcome = weightedInvestments.Simulate(InvestmentGoals, SimulationSize);
                results[weights] = outcome;
            });

            bestWeights = new Weights(GetNewWeightsFromResults(investments.Length, results));
            iterations++;
        }

        return WeightedInvestments.From(investments, bestWeights);
    }

    private Weights AverageWeights(List<Weights> w)
    {
        var numInvestments = w[0].Count;
        double[] newWeights = new double[numInvestments];
        for (int i = 0; i < numInvestments; i++)
        {
            for (int j = 0; j < w.Count; j++)
            {
                newWeights[i] += w[j][i];
            }
            newWeights[i] /= w.Count;
        }

        return new Weights(newWeights);
    }

    private static double[] GetNewWeightsFromResults(int numInvestments, ConcurrentDictionary<Weights, double> results)
    {
        List<KeyValuePair<Weights, double>> resultsList = SortResults(results);

        Weights[] bestWeightsLists = resultsList[..Math.Max(results.Count / 4, 1)]
            .Select(res => res.Key).ToArray();

        Weights[] worstWeightsLists = resultsList[(resultsList.Count - Math.Max(results.Count / 4, 1))..]
            .Select(res => res.Key).ToArray();

        double[] newBestWeights = new double[numInvestments];
        double[] newWorstWeights = new double[numInvestments];
        double[] newWeights = new double[numInvestments];
        for (int i = 0; i < numInvestments; i++)
        {
            for (int j = 0; j < bestWeightsLists.Length; j++)
            {
                newBestWeights[i] += bestWeightsLists[j][i];
                newWorstWeights[i] += worstWeightsLists[j][i];
            }
            newBestWeights[i] /= bestWeightsLists.Length;
            newWorstWeights[i] /= worstWeightsLists.Length;
            newWeights[i] = 2 * newBestWeights[i] - 1 * newWorstWeights[i];
        }

        return newBestWeights;
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
            var reducedWeights = new Weights([.. previousBest.ToArray()[1..]]);
            IEnumerable<Weights> weightsList = GetWeightsToTest(reducedWeights, searchWidth);

            foreach (var weights in weightsList)
            {
                yield return weights.Prepend(lower);
                yield return weights.Prepend(upper);
            }
        }
    }
}
