using AutoMapper;
using DataAnalyzer.Model;
using DataAnalyzingService.Model;
using lib.DTO;
using Neo4jClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IMapper>
    (
    new Mapper(
        new MapperConfiguration(cfg => {
            cfg.CreateMap<PrescriptionDto, PrescriptionNDto>();
            cfg.CreateMap<PersonDto, PersonNDto>()
            .ForMember(dist => dist.Latitude, opt => opt.MapFrom(src => src.Address.Latitude))
            .ForMember(dist => dist.Longitude, opt => opt.MapFrom(src => src.Address.Longitude))
            .ForMember(dist => dist.StreetNumber, opt => opt.MapFrom(src => src.Address.StreetNumber))
            .ForMember(dist => dist.StreetName, opt => opt.MapFrom(src => src.Address.StreetName))
            .ForMember(dist => dist.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
            .ReverseMap();
            cfg.CreateMap<PharmacyDto, PharmacyNDto>()
            .ForMember(dist => dist.Latitude, opt => opt.MapFrom(src => src.Address.Latitude))
            .ForMember(dist => dist.Longitude, opt => opt.MapFrom(src => src.Address.Longitude))
            .ForMember(dist => dist.StreetNumber, opt => opt.MapFrom(src => src.Address.StreetNumber))
            .ForMember(dist => dist.StreetName, opt => opt.MapFrom(src => src.Address.StreetName))
            .ForMember(dist => dist.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
            .ReverseMap();
            cfg.CreateMap<ConsultationDto,ConsultationNDto>()
            .ForMember(dist => dist.Latitude, opt => opt.MapFrom(src => src.GeoPoint.Latitude))
            .ForMember(dist => dist.Longitude, opt => opt.MapFrom(src => src.GeoPoint.Longitude))

            .ReverseMap();
        }
       )

    )) ;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string prescriptionServiceConnectionString = builder.Configuration.GetConnectionString("prescription_service");
string consultationServiceConnectionString = builder.Configuration.GetConnectionString("consultation_service");
builder.Services.AddHttpClient("PrescriptionClient", conf => conf.BaseAddress = new(prescriptionServiceConnectionString));
builder.Services.AddHttpClient("ConsultationClient", conf => conf.BaseAddress = new(consultationServiceConnectionString));
var cs = builder.Configuration.GetConnectionString("bolt");
var client = new BoltGraphClient(new Uri(cs), "neo4j", "2wsxcde3");
await client.ConnectAsync();
builder.Services.AddSingleton<IGraphClient>(client);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});



app.UseAuthorization();

app.MapControllers();

app.Run();
