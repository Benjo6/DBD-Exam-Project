using AutoMapper;
using lib.Configurations;
using lib.DTO;
using lib.Models;
using PrescriptionService.DAP;
using PrescriptionService.Data;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Data.Storage;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string adminConnString = builder.Configuration.GetConnectionString("postgres_admin");
string customConnString = builder.Configuration.GetConnectionString("postgres_custom_user");

RedisCacheConfig settings = builder.Configuration.GetSection(RedisCacheConfig.ConfigKey).Get<RedisCacheConfig>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(settings.EndPoints));

builder.Services.AddScoped<IRedisCache, RedisCache>();
builder.Services.AddScoped<IPrescriptionStorage, PrescriptionStorage>();

builder.Services.AddNpgsql<PostgresContext>(builder.Configuration.GetConnectionString("postgres_admin"));
builder.Services.AddSingleton<IPrescriptionRepo>(new DapperPrescriptionRepo(adminConnString, customConnString));
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IAsyncRepository<Patient>, PatientRepository>();
builder.Services.AddScoped<IAsyncRepository<Pharmacy>, PharmacyRepository>();

builder.Services.AddSingleton<IMapper>(
    new Mapper(
        new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<Prescription, PrescriptionDto>()
                    .ForMember(dist => dist.Patient, opt => opt.MapFrom(src => src.PrescribedToNavigation))
                    .ForMember(dist => dist.DoctorId, opt => opt.MapFrom(src => src.PrescribedBy))
                    .ReverseMap();
                cfg.CreateMap<Medicine, MedicineDto>()
                    .ReverseMap();
                cfg.CreateMap<Patient, PatientDto>()
                    .ForMember(dist => dist.FirstName, opt => opt.MapFrom(src => src.PersonalData.FirstName))
                    .ForMember(dist => dist.LastName, opt => opt.MapFrom(src => src.PersonalData.LastName))
                    .ForMember(dist => dist.Email, opt => opt.MapFrom(src => src.PersonalData.Email))
                    .ForMember(dist => dist.CphNumber, opt => opt.MapFrom(src => src.Cpr))
                    .ReverseMap();
                cfg.CreateMap<Address, AddressDto>()
                    .ReverseMap();
                cfg.CreateMap<Pharmacy, PharmacyDto>()
                    .ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.PharmacyName))
                    .ReverseMap();
            })));

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

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.Run();
