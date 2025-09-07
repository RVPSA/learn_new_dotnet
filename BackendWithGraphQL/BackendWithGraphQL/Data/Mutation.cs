using BackendWithGraphQL.DAL;
using BackendWithGraphQL.Models.Entities;

namespace BackendWithGraphQL.Data;

public class Mutation
{
    public Employee AddEmployee([Service] ApplicationDBContext context,string name, string gender, int salary)
    {
        var employee = new Employee()
        {
            Name = name,
            Gender = gender,
            Salary = salary
        };
        context.Employees.Add(employee);
        context.SaveChanges();
        return employee;
        
    }

    public Employee UpdateEmployee([Service] ApplicationDBContext context,string name, string gender, int salary, int id)
    {
        var employee = context.Employees.Find(id);
        if (employee == null) 
            return null;
        employee.Name = name;
        employee.Gender = gender;
        employee.Salary = salary;
        context.SaveChanges();
        return employee;
    }

    public Employee DeleteEmployee([Service] ApplicationDBContext context,int id)
    {
        var employee = context.Employees.Find(id);
        if (employee == null)
            return null;
        context.Employees.Remove(employee);
        context.SaveChanges();
        return employee;
    }
}