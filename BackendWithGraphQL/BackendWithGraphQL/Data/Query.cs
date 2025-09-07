using BackendWithGraphQL.DAL;
using BackendWithGraphQL.Models.Entities;

namespace BackendWithGraphQL.Data;

public class Query
{

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public List<Employee> GetAllEmployees([Service] ApplicationDBContext context)
    {
        return context.Employees.ToList();
    }
    
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Employee GetEmployeeById([Service] ApplicationDBContext context,int id)
    {
        var employee = context.Employees.Find(id);
        if (employee == null)
            return null;
        return employee;
    }
    
}
