using System.Collections;

namespace InvestmentDistribution;

public class InvestmentGoals(List<InvestmentGoal> goalList, int numberOfYears) : IEnumerable<InvestmentGoal>
{
    private readonly List<InvestmentGoal> _goalList = goalList;
    public int NumberOfYears { get; } = numberOfYears;

    public IEnumerator<InvestmentGoal> GetEnumerator() => _goalList.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
