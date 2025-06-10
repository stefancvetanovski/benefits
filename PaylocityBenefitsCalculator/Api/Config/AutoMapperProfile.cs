using Api.Dtos.Employee;
using AutoMapper;

namespace Api.Config;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Mapping configuration from GetEmployeeDto to GetEmployeePaycheckDto
        CreateMap<GetEmployeeDto, GetEmployeePaycheckDto>();
    }
}