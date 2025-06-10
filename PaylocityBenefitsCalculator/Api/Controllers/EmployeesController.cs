using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeesService _employeesService;

    public EmployeesController(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        return await _employeesService.GetEmployeeByIdWithDependentsAsync(id);
    }
    
    [SwaggerOperation(Summary = "Get employee paycheck by employee id")]
    [HttpGet("{id}/paycheck")]
    public async Task<ActionResult<ApiResponse<GetEmployeePaycheckDto>>> GetPaycheck(int id)
    {
        return await _employeesService.GetEmployeePaycheckByEmployeeIdAsync(id);
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        return await _employeesService.GetEmployeesWithDependentsAsync();
    }
}
