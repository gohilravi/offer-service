using FluentAssertions;
using OfferService.Domain.Enums;

namespace OfferService.UnitTests.Domain;

public class OfferStatusTests
{
    [Theory]
    [InlineData("offered")]
    [InlineData("assigned")]
    [InlineData("canceled")]
    public void IsValid_ValidStatus_ReturnsTrue(string status)
    {
        // Act
        var result = OfferStatus.IsValid(status);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData(null)]
    public void IsValid_InvalidStatus_ReturnsFalse(string status)
    {
        // Act
        var result = OfferStatus.IsValid(status);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("offered", "assigned", true)]
    [InlineData("offered", "canceled", true)]
    [InlineData("assigned", "canceled", true)]
    [InlineData("canceled", "offered", false)]
    [InlineData("canceled", "assigned", false)]
    [InlineData("assigned", "offered", false)]
    public void CanTransitionTo_VariousTransitions_ReturnsExpectedResult(
        string currentStatus, string newStatus, bool expected)
    {
        // Act
        var result = OfferStatus.CanTransitionTo(currentStatus, newStatus);

        // Assert
        result.Should().Be(expected);
    }
}