using BackendWithGraphQL.DAL;
using BackendWithGraphQL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddGraphQLServer()
    .RegisterDbContextFactory<ApplicationDBContext>()
    .AddQueryType<Query>().AddMutationType<Mutation>()
    .AddProjections().AddFiltering().AddSorting();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactGraphQL", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
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
app.UseCors("ReactGraphQL");
//app.UseAuthorization();
app.MapGraphQL("/graphql");
app.MapControllers();


app.Run();
