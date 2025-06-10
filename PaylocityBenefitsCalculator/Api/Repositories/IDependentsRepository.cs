using Api.Models;

namespace Api.Repositories;

public interface IDependentsRepository
{
    List<Dependent> GetAllDependents();
    Dependent? GetDependentById(int id);
    List<Dependent>? GetDependentByEmployeeId(int id);
    void AddDependent(Dependent dependent);
    void UpdateDependent(int id, Dependent updatedDependent);
    void DeleteDependent(int id);
}