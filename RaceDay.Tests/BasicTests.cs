using Xunit;

namespace RaceDay.Tests;

public class BasicTests
{
    [Theory]
    [InlineData("run", true)]
    [InlineData("walk", true)]
    [InlineData("cycle", true)]
    [InlineData("swim", false)]
    public void EventTypeMustBeRunWalkOrCycle(string eventType, bool expected)
    {
        var valid = new[] { "run", "walk", "cycle" }.Contains(eventType);
        Assert.Equal(expected, valid);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-2, false)]
    public void FinishingPositionMustBePositive(int position, bool expected)
        => Assert.Equal(expected, position >= 1);
}
