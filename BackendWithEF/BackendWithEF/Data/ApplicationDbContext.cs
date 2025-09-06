using BackendWithEF.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendWithEF.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
        
    }

    public DbSet<Car> Cars { get; set; }
}