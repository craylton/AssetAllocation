namespace InvestmentDistribution;

public class SimulationAccuracy(double threshold, int samplesPerSimulation)
{
    public double Threshold { get; } = threshold;
    public int SamplesPerSimulation { get; } = samplesPerSimulation;

    public static SimulationAccuracy VeryLow = new(1e-3, 10);
    public static SimulationAccuracy Low = new(1e-4, 20);
    public static SimulationAccuracy Normal = new(1e-5, 100);
    public static SimulationAccuracy High = new(1e-5, 400);
    public static SimulationAccuracy VeryHigh = new(1e-5, 1000);
}
