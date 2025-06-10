using System.Collections.Concurrent;
using Api.Models;

namespace Api.Repositories;

public class EmployeeInMemoryRepository : IEmployeesRepository
{
    private readonly ConcurrentDictionary<int, Employee> _employees;

    public EmployeeInMemoryRepository()
    {
        _employees = new ConcurrentDictionary<int, Employee>();

        // Seed data
        _employees.TryAdd(1, new Employee
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        });

        _employees.TryAdd(2, new Employee
        {
            Id = 2,
            FirstName = "Ja",
            LastName = "Morant",
            Salary = 92365.22m,
            DateOfBirth = new DateTime(1999, 8, 10)
        });

        _employees.TryAdd(3, new Employee
        {
            Id = 3,
            FirstName = "Michael",
            LastName = "Jordan",
            Salary = 143211.12m,
            DateOfBirth = new DateTime(1963, 2, 17)
        });
    }

    public List<Employee> GetAllEmployees() => _employees.Values.ToList();

    public Employee GetEmployeeById(int id)
    {
        if (!_employees.TryGetValue(id, out var employee))
        {
            throw new ResourceNotFoundException($"Employee with id {id} not found.");
        }

        return employee;
    }

    public void AddEmployee(Employee employee)
    {
        employee.Id = _employees.Keys.Count > 0 ? _employees.Keys.Max() + 1 : 1;
        _employees.TryAdd(employee.Id, employee);
    }

    public void UpdateEmployee(int id, Employee updatedEmployee)
    {
        if (_employees.ContainsKey(id))
        {
            _employees[id] = updatedEmployee;
        }
    }

    public void DeleteEmployee(int id)
    {
        _employees.TryRemove(id, out _);
    }
}