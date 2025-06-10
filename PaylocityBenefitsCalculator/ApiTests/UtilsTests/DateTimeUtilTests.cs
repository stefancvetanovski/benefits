using System;
using Api.Utils;
using Xunit;

namespace ApiTests.UtilsTests;

public class DateTimeUtilTests
{
    [Fact]
    public void ExactYearsDifference_WhenDatesAreSame_ReturnsZero()
    {
        // Arrange
        var startDate = new DateTime(2023, 5, 15);
        var endDate = new DateTime(2023, 5, 15);

        // Act
        var result = DateTimeUtils.GetExactYearsDifference(startDate, endDate);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ExactYearsDifference_WhenFullYearHasPassed_ReturnsOne()
    {
        // Arrange
        var startDate = new DateTime(2022, 5, 15);
        var endDate = new DateTime(2023, 5, 15);

        // Act
        var result = DateTimeUtils.GetExactYearsDifference(startDate, endDate);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ExactYearsDifference_WhenLessThanAYearHasPassed_ReturnsZero()
    {
        // Arrange
        var startDate = new DateTime(2022, 5, 15);
        var endDate = new DateTime(2023, 5, 14);

        // Act
        var result = DateTimeUtils.GetExactYearsDifference(startDate, endDate);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ExactYearsDifference_WhenLeapYear_HandlesCorrectly()
    {
        // Arrange
        var startDate = new DateTime(2020, 2, 29); // Leap year date
        var endDate = new DateTime(2021, 2, 28); // Approx. one year later

        // Act
        var result = DateTimeUtils.GetExactYearsDifference(startDate, endDate);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ExactYearsDifference_WhenAcrossLeapYear_HandlesCorrectly()
    {
        // Arrange
        var startDate = new DateTime(2020, 2, 29); // Leap year date
        var endDate = new DateTime(2021, 3, 1); // More than one year later (leap year handled)

        // Act
        var result = DateTimeUtils.GetExactYearsDifference(startDate, endDate);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ExactYearsDifference_WhenNegativeYearDifference_ReturnsCorrectYears()
    {
        // Arrange
        var startDate = new DateTime(2023, 5, 15);
        var endDate = new DateTime(2000, 5, 15);

        // Act
        var result = DateTimeUtils.GetExactYearsDifference(endDate, startDate);

        // Assert
        Assert.Equal(23, result);
    }

    [Fact]
    public void ExactYearsDifference_WhenEndDateOccursBeforeAnniversary_ReturnsCorrectYears()
    {
        // Arrange
        var startDate = new DateTime(2000, 5, 15);
        var endDate = new DateTime(2023, 4, 10);

        // Act
        var result = DateTimeUtils.GetExactYearsDifference(startDate, endDate);

        // Assert
        Assert.Equal(22, result);
    }

    [Fact]
    public void ExactYearsDifference_WhenEndDateOccursAfterAnniversary_ReturnsCorrectYears()
    {
        // Arrange
        var startDate = new DateTime(2000, 5, 15);
        var endDate = new DateTime(2023, 6, 10);

        // Act
        var result = DateTimeUtils.GetExactYearsDifference(startDate, endDate);

        // Assert
        Assert.Equal(23, result);
    }

    [Fact]
    public void ExactYearsDifference_WhenDatesSpanDecades_ReturnsCorrectDifference()
    {
        // Arrange
        var startDate = new DateTime(1980, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        // Act
        var result = DateTimeUtils.GetExactYearsDifference(startDate, endDate);

        // Assert
        Assert.Equal(43, result);
    }
}