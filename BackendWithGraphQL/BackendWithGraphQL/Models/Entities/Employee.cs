using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BackendWithGraphQL.Models.Entities;

public class Employee
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public int Salary { get; set; }
}