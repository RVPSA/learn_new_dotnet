using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Adding Logger
//With default configurations
//var logger = new LoggerConfiguration().
//    ReadFrom.Configuration(builder.Configuration).//configures the logger based on the application's configuration.
                                                  //use builder.Configuration object to read the configuration settings and apply them to the logger.
//    Enrich.FromLogContext().CreateLogger();
//builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(logger);

//Log configuratins from file
var logger = new LoggerConfiguration() //creates a new LoggerConfiguration object that will be used to configure the Serilog logger.
    .ReadFrom.Configuration(new ConfigurationBuilder()
    .AddJsonFile("seri-log.config.json").Build())//Read Configuration from the seperate json file
    .Enrich.FromLogContext() //adds contextual information to log events,log events to be enriched with additional information,
                             //such as the name of the current method or the user that initiated the event.
    .CreateLogger(); //create the Serilog logger.
builder.Logging.ClearProviders(); //called on an instance of ILoggerFactory in a .NET application to remove all the logging providers from the logging pipeline.
builder.Logging.AddSerilog(logger); //add a Serilog logger to the logging pipeline of a .NET application.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
