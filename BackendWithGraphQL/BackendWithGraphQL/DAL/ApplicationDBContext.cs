using BackendWithGraphQL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendWithGraphQL.DAL;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
    {
        
    }
    
    public DbSet<Employee> Employees { get; set; }
}