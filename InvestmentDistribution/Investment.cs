namespace InvestmentDistribution;

public class Investment(string name, double mean, double standardDeviation)
{
    public string Name { get; } = name;
    public double Mean { get; } = mean;
    public double StandardDeviation { get; } = standardDeviation;
}