using System;
using System.Configuration;
using System.Threading.Tasks;
using AutoMapper;
using lib.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neo4jClient;
using Neo4JDataSupplier.Model;

namespace Neo4JDataSupplier
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            var client = new BoltGraphClient(new Uri("bolt://localhost:7687"), "neo4j", "2wsxcde3");
            await client.ConnectAsync();
            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, app) =>
                {
                    
                    app.AddJsonFile("appsettings.json", optional: true, true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<Neo4jClient>();
                    services.AddSingleton<IGraphClient>(client);
                    services.AddSingleton<IMapper>(
                        new Mapper(
                            new MapperConfiguration(cfg => cfg.CreateMap<PrescriptionDto, PrescriptionNDto>())));
                }).UseConsoleLifetime();
            
          
       

            var host = builder.Build();

       

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var myService = services.GetRequiredService<Neo4jClient>();
                    var result = await myService.Run();

                    Console.WriteLine($"{string.Join(", ", result)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }


        }


    }
}
