using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repositories;
using Api.Utils;
using AutoMapper;

namespace Api.Services;

public class EmployeesService : IEmployeesService
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly IDependentsRepository _dependentsRepository;
    private readonly IMapper _mapper;

    public EmployeesService(IEmployeesRepository employeesRepository, IDependentsRepository dependentsRepository,
        IMapper mapper)
    {
        _employeesRepository = employeesRepository;
        _dependentsRepository = dependentsRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetEmployeeDto>>> GetEmployeesWithDependentsAsync()
    {
        // Get all employees
        var employees = _employeesRepository.GetAllEmployees();

        // Project into GetEmployeeDto with dependents
        var employeeList = employees.Select(employee => new GetEmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Salary = employee.Salary,
            DateOfBirth = employee.DateOfBirth,
            Dependents = _dependentsRepository.GetDependentByEmployeeId(employee.Id)?.Select(dependent =>
                new GetDependentDto
                {
                    Id = dependent.Id,
                    FirstName = dependent.FirstName,
                    LastName = dependent.LastName,
                    DateOfBirth = dependent.DateOfBirth,
                    Relationship = dependent.Relationship
                }).ToList() ?? new List<GetDependentDto>()
        });

        return new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employeeList.ToList(),
            Success = true
        };
    }

    public async Task<ApiResponse<GetEmployeeDto>> GetEmployeeByIdWithDependentsAsync(int id)
    {
        // Get the employee by ID
        var employee = _employeesRepository.GetEmployeeById(id);
        if (employee == null)
        {
            throw new ResourceNotFoundException($"Employee with id {id} not found.");
        }

        // Project into GetEmployeeDto with dependents
        var employeeInfo = new GetEmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Salary = employee.Salary,
            DateOfBirth = employee.DateOfBirth,
            Dependents = _dependentsRepository.GetDependentByEmployeeId(employee.Id)?.Select(dependent =>
                new GetDependentDto
                {
                    Id = dependent.Id,
                    FirstName = dependent.FirstName,
                    LastName = dependent.LastName,
                    DateOfBirth = dependent.DateOfBirth,
                    Relationship = dependent.Relationship
                }).ToList() ?? new List<GetDependentDto>()
        };

        return new ApiResponse<GetEmployeeDto>
        {
            Data = employeeInfo,
            Success = true
        };
    }

    public async Task<ApiResponse<GetEmployeePaycheckDto>> GetEmployeePaycheckByEmployeeIdAsync(int id)
    {
        var employeeInfo = GetEmployeeByIdWithDependentsAsync(id);
        var response = _mapper.Map<GetEmployeePaycheckDto>(employeeInfo.Result.Data);
        response.Benefits = CalculateYearlyBenefits(response);
        return new ApiResponse<GetEmployeePaycheckDto>()
        {
            Data = response,
            Success = true
        };
    }

    public decimal CalculateYearlyBenefits(GetEmployeePaycheckDto employee)
    {
        // the base monthly benefits is 1000
        var totalMonthlyBenefits = 1000m;
        // adding benefits for dependents
        totalMonthlyBenefits += CalculateMonthlyDependentBenefits(employee.Dependents);
        
        // adding benefits if salary over some threshold
        var extraBenefits = CalculateExtraBenefitsBasedOnSalary(employee);
        
        // calculated benefits are monthly, we are multiplying that by 12 to get the yearly
        // and then dividing by 26 to get the payslip benefits
        return totalMonthlyBenefits * 12 + extraBenefits;
    }

    public decimal CalculateExtraBenefitsBasedOnSalary(GetEmployeePaycheckDto employee)
    {
        if (employee.Salary <= 80000m)
        {
            return 0m;
        }
        
        return 0.02m * employee.Salary;
    }

    public decimal CalculateMonthlyDependentBenefits(ICollection<GetDependentDto> dependents)
    {
        if (!dependents.Any())
        {
            // if no dependents, benefits for them will be 0
            return 0m;
        }

        var totalMonthlyDependantBenefits = 0m;
        foreach (var dependent in dependents)
        {
            // base benefit for dependent
            totalMonthlyDependantBenefits += 600m;
            if (DateTimeUtils.GetExactYearsDifference(dependent.DateOfBirth, DateTime.Now) > 50)
            {
                totalMonthlyDependantBenefits += 200m;
            }
        }

        return totalMonthlyDependantBenefits;
    }

    public async Task<ApiResponse<GetDependentDto>> GetDependentByIdAsync(int id)
    {
        var dependent = _dependentsRepository.GetDependentById(id);
        if (dependent == null)
        {
            throw new ResourceNotFoundException($"Dependent with id {id} not found.");
        }

        var dependentInfo = new GetDependentDto
        {
            Id = dependent.Id,
            FirstName = dependent.FirstName,
            LastName = dependent.LastName,
            DateOfBirth = dependent.DateOfBirth,
            Relationship = dependent.Relationship
        };

        return new ApiResponse<GetDependentDto>
        {
            Data = dependentInfo,
            Success = true
        };
    }

    public async Task<ApiResponse<List<GetDependentDto>>> GetDependentsAsync()
    {
        var dependents = _dependentsRepository.GetAllDependents();

        var dependentsInfo = dependents.Select(x => new GetDependentDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            DateOfBirth = x.DateOfBirth,
            Relationship = x.Relationship
        });

        return new ApiResponse<List<GetDependentDto>>
        {
            Data = dependentsInfo.ToList(),
            Success = true
        };
    }
}