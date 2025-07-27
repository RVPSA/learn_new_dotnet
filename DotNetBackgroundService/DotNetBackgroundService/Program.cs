using DotNetBackgroundService.BackgroundServices;
using DotNetBackgroundService.BackgroundServices.IBackground;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//builder.Services.AddHostedService<BackgroundWithIHosted>();
builder.Services.AddHostedService<BackgroundWithBackgroundService>();
builder.Services.AddSingleton<IBackgroundTaskQueue,BackgroundTaskQueue>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();