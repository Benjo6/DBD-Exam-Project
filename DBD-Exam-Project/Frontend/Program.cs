using Frontend.Authentication;
using Frontend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<UserProvider>();

builder.Services.AddAntDesign();

string consultationConnectionString = builder.Configuration.GetConnectionString("ConsultationService");
builder.Services.AddHttpClient(
    "ConsultationClient",
    conf => conf.BaseAddress = new(consultationConnectionString));

string prescriptionConnectionString = builder.Configuration.GetConnectionString("PrescriptionService");
builder.Services
    .AddHttpClient(
    "PrescriptionClient", 
    conf => conf.BaseAddress = new(prescriptionConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
