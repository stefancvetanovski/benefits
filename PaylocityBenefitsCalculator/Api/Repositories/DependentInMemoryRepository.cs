using System.Collections.Concurrent;
using Api.Models;

namespace Api.Repositories;

public class DependentInMemoryRepository : IDependentsRepository
{
    private readonly ConcurrentDictionary<int, List<Dependent>> _dependents;

    public DependentInMemoryRepository()
    {
        _dependents = new ConcurrentDictionary<int, List<Dependent>>();
        _dependents.TryAdd(2, new List<Dependent>()
        {
            new Dependent
            {
                Id = 1, FirstName = "Spouse", LastName = "Morant",
                DateOfBirth = new DateTime(1998, 3, 3), Relationship = Relationship.Spouse, EmployeeId = 2
            },
            new Dependent
            {
                Id = 2, FirstName = "Child1", LastName = "Morant",
                DateOfBirth = new DateTime(2020, 6, 23), Relationship = Relationship.Child, EmployeeId = 2
            },
            new Dependent
            {
                Id = 3, FirstName = "Child2", LastName = "Morant",
                DateOfBirth = new DateTime(2021, 5, 18), Relationship = Relationship.Child, EmployeeId = 2
            }
        });
        _dependents.TryAdd(3, new List<Dependent>()
        {
            new Dependent
            {
                Id = 4, FirstName = "DP", LastName = "Jordan",
                DateOfBirth = new DateTime(1974, 1, 2), Relationship = Relationship.DomesticPartner, EmployeeId = 3
            }
        });
    }

    public List<Dependent> GetAllDependents()
    {
        // Returns all dependents across all employees
        return _dependents.Values.SelectMany(dependents => dependents).ToList();
    }

    public Dependent? GetDependentById(int id)
    {
        // Search for the dependent by ID across all employees
        return _dependents.Values.SelectMany(dependents => dependents)
            .FirstOrDefault(dependent => dependent.Id == id);
    }

    public List<Dependent>? GetDependentByEmployeeId(int id)
    {
        return _dependents.GetValueOrDefault(id);
    }

    public void AddDependent(Dependent dependent)
    {
        if (!_dependents.ContainsKey(dependent.EmployeeId))
        {
            // If employee ID doesn't exist, initialize its list
            _dependents.TryAdd(dependent.EmployeeId, new List<Dependent>());
        }

        var dependentsList = _dependents[dependent.EmployeeId];

        // Ensure unique ID for the new dependent
        dependent.Id = dependentsList.Count > 0 ? dependentsList.Max(d => d.Id) + 1 : 1;

        // Add the new dependent
        dependentsList.Add(dependent);
    }

    public void UpdateDependent(int id, Dependent updatedDependent)
    {
        foreach (var employeeDependents in _dependents.Values)
        {
            var index = employeeDependents.FindIndex(dependent => dependent.Id == id);
            if (index >= 0)
            {
                // Update the dependent in place
                employeeDependents[index] = updatedDependent;
                return;
            }
        }
    }

    public void DeleteDependent(int id)
    {
        foreach (var dependents in _dependents.Values)
        {
            var dependentToRemove = dependents.FirstOrDefault(dependent => dependent.Id == id);
            if (dependentToRemove != null)
            {
                dependents.Remove(dependentToRemove);
                return;
            }
        }
    }
}