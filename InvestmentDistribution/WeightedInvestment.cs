﻿using MathNet.Numerics.Distributions;

namespace InvestmentDistribution;

public class WeightedInvestment(string name, double mean, double standardDeviation, double weight)
    : Investment(name, mean, standardDeviation)
{
    public double Weight { get; private set; } = weight;
    public double WeightedMean => Weight * Mean;
    public double WeightedStdDev => Weight * StandardDeviation;
    public Normal Pdf => new(mean, StandardDeviation);

    public static WeightedInvestment From(Investment investment, double weight) =>
        new WeightedInvestment(investment.Name, investment.Mean, investment.StandardDeviation, weight);

    public override string ToString() =>
        $"{Name}: {Weight * 100:0.00}% (N({Mean}, {StandardDeviation}))";
}