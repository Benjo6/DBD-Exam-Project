using ConsultationService.Services;
using lib.Converter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neo4jClient;
using PrescriptionService.DAP;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var client = new BoltGraphClient(new Uri("bolt://localhost:7687"), "neo4j", "2wsxcde3");
client.ConnectAsync();

builder.Services.AddSingleton<IGraphClient>(client);
builder.Services.AddScoped<IConsultationService, MongoConsultationService>();
var connectionString = "Host=localhost;Port=15432;Database=prescription_db;Include Error Detail=true;Username=prescription_user;Password=prescription_pw";
string customConnString = "Host=localhost;Port=5432;Database=prescription_db;Include Error Detail=true;Username={user};Password={pass}";
builder.Services.AddSingleton<IPrescriptionRepo>(new DapperPrescriptionRepo(connectionString, customConnString));


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
