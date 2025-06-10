using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repositories;

namespace Api.Services;

public class EmployeesService : IEmployeesService
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly IDependentsRepository _dependentsRepository;

    public EmployeesService(IEmployeesRepository employeesRepository, IDependentsRepository dependentsRepository)
    {
        _employeesRepository = employeesRepository;
        _dependentsRepository = dependentsRepository;
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