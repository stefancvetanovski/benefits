using Api.Models;

namespace Api.Repositories;

public interface IEmployeesRepository
{
    List<Employee> GetAllEmployees();
    Employee GetEmployeeById(int id);
    void AddEmployee(Employee employee);
    void UpdateEmployee(int id, Employee updatedEmployee);
    void DeleteEmployee(int id);
}