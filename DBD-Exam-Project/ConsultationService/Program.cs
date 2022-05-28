using System.Text.Json.Serialization;
using ConsultationService;
using ConsultationService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<IConsultationService, MongoConsultationService>();
builder.Services.AddScoped<IConsultationMetadataService, MongoConsultationMetadataService>();

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
