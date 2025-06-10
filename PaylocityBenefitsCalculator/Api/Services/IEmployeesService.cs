using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services;

public interface IEmployeesService
{
    Task<ApiResponse<List<GetEmployeeDto>>> GetEmployeesWithDependentsAsync();
    Task<ApiResponse<GetEmployeeDto>> GetEmployeeByIdWithDependentsAsync(int id);
    Task<ApiResponse<GetEmployeePaycheckDto>> GetEmployeePaycheckByEmployeeIdAsync(int id);
    Task<ApiResponse<GetDependentDto>> GetDependentByIdAsync(int id);
    Task<ApiResponse<List<GetDependentDto>>> GetDependentsAsync();

}