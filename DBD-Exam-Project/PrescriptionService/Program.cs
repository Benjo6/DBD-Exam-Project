using lib.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrescriptionService.DAP;
using PrescriptionService.Data;
using PrescriptionService.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string adminConnString = builder.Configuration.GetConnectionString("postgres_admin");
string customConnString = builder.Configuration.GetConnectionString("postgres_custom_user");

builder.Services.AddNpgsql<PostgresContext>(builder.Configuration.GetConnectionString("postgres_admin"));
builder.Services.AddSingleton<IPrescriptionRepo>(new DapperPrescriptionRepo(adminConnString, customConnString));
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IAsyncRepository<Patient>, PatientRepository>();
builder.Services.AddScoped<IAsyncRepository<Pharmacy>, PharmacyRepository>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});


app.UseAuthorization();

app.MapControllers();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.Run();
