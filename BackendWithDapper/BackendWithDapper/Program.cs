using BackendWithDapper.Extensions;
using DAL.Core;
using Models.Settings;

IAppSettings _appSettings;
var builder = WebApplication.CreateBuilder(args);

//Adding AppSettings
builder.AddAppSettings();
_appSettings = builder.Configuration.AddConfigurations();
builder.Services.AddSingleton(_appSettings);

// Add services to the container.
builder.Services.AddControllers();

builder.AddServiceExtensions();
builder.AddRepositoryExtensions();
builder.Services.AddScoped<IDapperDbConnection, DapperDbConnection>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = _appSettings.RedisConfigurations.RedisURL;
    option.InstanceName = _appSettings.RedisConfigurations.Instances.Employee;
});


builder.Services.AddCors(options=> {
    options.AddPolicy(name: "MyAllowedSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173").
            AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("MyAllowedSpecificOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();
