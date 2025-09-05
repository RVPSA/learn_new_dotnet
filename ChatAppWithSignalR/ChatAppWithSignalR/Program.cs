using ChatAppWithSignalR.DataService;
using ChatAppWithSignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("reactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddSingleton<ShareDb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapHub<ChatHub>("/Chat");
app.UseCors("reactApp");
app.Run();

