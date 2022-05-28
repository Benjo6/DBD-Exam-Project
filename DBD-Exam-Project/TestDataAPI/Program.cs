using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestDataAPI;
using TestDataAPI.Context;
using TestDataAPI.DAP;
using TestDataAPI.Seeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Error);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddDbContext<PrescriptionContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("postgres") ?? throw new Exception("Postgres connection not set")));
builder.Services.AddTransient<DbSeeder>();
builder.Services.AddSingleton<IPrescriptionRepo>(new DapperPrescriptionRepo(builder.Configuration.GetConnectionString("postgres") ?? throw new Exception("Postgres connection not set")));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseAuthorization();
app.MapControllers();

app.Run();