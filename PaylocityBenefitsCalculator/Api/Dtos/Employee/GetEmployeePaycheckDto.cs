namespace Api.Dtos.Employee;

public class GetEmployeePaycheckDto : GetEmployeeDto
{
    public decimal Benefits { get; set; }
    public decimal PaycheckBenefits => Benefits / 26m;
    public decimal PaycheckSalary => Salary / 26m;
    
}