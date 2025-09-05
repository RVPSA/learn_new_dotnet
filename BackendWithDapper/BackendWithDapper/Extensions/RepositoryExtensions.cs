using DAL.Repositories.IRepository;
using DAL.Repositories.Repository;

namespace BackendWithDapper.Extensions;

public static class RepositoryExtensions
{
    public static void AddRepositoryExtensions(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        var a =builder.Configuration["RedisConfigurations:RedisURL"];
    }
}