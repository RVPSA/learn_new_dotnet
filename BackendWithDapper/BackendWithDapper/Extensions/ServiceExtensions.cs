using Redis;
using Service.Services.IService;
using Service.Services.Service;

namespace BackendWithDapper.Extensions;

public static class ServiceExtensions
{
    public static void AddServiceExtensions(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IRedisCacheService,RedisCacheService>();
    }
}