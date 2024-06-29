using System.Collections;

namespace InvestmentDistribution;

public class Weights(IList<double> weights) : IList<double>
{
    private readonly IList<double> _weights = weights;

    public int Count => _weights.Count;

    public bool IsReadOnly => false;

    public double this[int index]
    {
        get => _weights[index];
        set => _weights[index] = value;
    }

    public Weights Normalise()
    {
        var sumOfWeights = _weights.Sum();
        return new Weights(_weights.Select(weight => weight / sumOfWeights).ToList());
    }

    public int IndexOf(double item) => _weights.IndexOf(item);
    public void Insert(int index, double item) => _weights[index] = item;
    public void RemoveAt(int index) => _weights.RemoveAt(index);
    public void Add(double item) => _weights.Add(item);
    public void Clear() => _weights.Clear();
    public bool Contains(double item) => _weights.Contains(item);
    public void CopyTo(double[] array, int arrayIndex) => _weights.CopyTo(array, arrayIndex);
    public bool Remove(double item) => _weights.Remove(item);
    public Weights Prepend(double item) => new Weights(_weights.Prepend(item).ToList());
    public IEnumerator<double> GetEnumerator() => _weights.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
