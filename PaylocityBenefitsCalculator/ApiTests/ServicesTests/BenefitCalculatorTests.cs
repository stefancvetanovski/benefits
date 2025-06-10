using System;
using System.Collections.Generic;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Services;
using Xunit;

namespace ApiTests.ServicesTests;

public class BenefitCalculatorTests
{
    [Theory]
    [InlineData(90000, 1800)] // Salary > 90000, expect 10% of 90000 as benefits
    [InlineData(5000, 0)] // Salary == 5000, no extra benefits
    [InlineData(3000, 0)] // Salary < 5000, no extra benefits
    public void CalculateExtraMonthlyBenefitsBasedOnSalary_WhenValidSalary_ReturnsExpectedBenefits(decimal salary,
        decimal expectedBenefits)
    {
        // Arrange
        var calculator = new EmployeesService(null, null, null);

        // Act
        var actualBenefits =
            calculator.CalculateExtraBenefitsBasedOnSalary(new GetEmployeePaycheckDto() { Salary = salary });

        // Assert
        Assert.Equal(expectedBenefits, actualBenefits);
    }

    [Theory]
    [InlineData(1, 0, 600)] // One dependent = 600 benefits
    [InlineData(2, 1, 2000)] // Three dependents = 2 * 600 + 1 * 800 = 2000
    [InlineData(5, 0, 3000)] // Five dependents = 5 * 600 = 3000
    public void CalculateMonthlyDependentBenefits_WhenDependentsProvided_ReturnsExpectedBenefits(
        int dependentLessThan50Count,
        int olderThanFiftyDependentCount, decimal expectedBenefits)
    {
        // Arrange
        var calculator = new EmployeesService(null, null, null);
        var dependents = GenerateDependents(dependentLessThan50Count, olderThanFiftyDependentCount);

        // Act
        var result = calculator.CalculateMonthlyDependentBenefits(dependents);

        // Assert
        Assert.Equal(expectedBenefits, result);
    }

    [Theory]
    [InlineData(1, 0, 90000, 21000.00)] // One dependent = 600 benefits
    [InlineData(2, 1, 90001, 37800.02)] // Three dependents = 2 * 600 + 1 * 800 = 2000
    [InlineData(5, 0, 40000, 48000)] // Five dependents = 5 * 600 = 3000
    public void CalculateBenefits_WhenDependentsProvided_ReturnsExpectedBenefits(int dependentLessThan50Count,
        int olderThanFiftyDependentCount, decimal salary, decimal expectedBenefits)
    {
        // Arrange
        var calculator = new EmployeesService(null, null, null);
        var employee = new GetEmployeePaycheckDto { Salary = salary };
        employee.Dependents = GenerateDependents(dependentLessThan50Count, olderThanFiftyDependentCount);

        // Act
        var result = calculator.CalculateYearlyBenefits(employee);

        // Assert
        Assert.Equal(expectedBenefits, result);
    }

    // Helper method to generate a list of dependents for testing
    private List<GetDependentDto> GenerateDependents(int count, int olderThanFifty = 0)
    {
        var dependents = new List<GetDependentDto>();
        for (int i = 0; i < count; i++)
        {
            dependents.Add(new GetDependentDto { DateOfBirth = DateTime.Now.AddYears(-5) });
        }

        if (olderThanFifty > 0)
        {
            for (int i = 0; i < olderThanFifty; i++)
            {
                dependents.Add(new GetDependentDto { DateOfBirth = DateTime.Now.AddYears(-55) });
            }
        }

        return dependents;
    }
}