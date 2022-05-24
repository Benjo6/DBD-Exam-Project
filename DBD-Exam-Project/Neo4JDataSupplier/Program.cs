using System;
using System.Configuration;
using System.Threading.Tasks;
using ConsultationService;
using ConsultationService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neo4jClient;
using PrescriptionService.DAP;

namespace Neo4JDataSupplier
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var client = new BoltGraphClient(new Uri("bolt://localhost:7687"), "neo4j", "2wsxcde3");
            await client.ConnectAsync();
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext,services) =>
                {
                   //services.Configure<DatabaseSettings>(Configuration.GetSection("ConnectionStrings"));
                    services.AddScoped<IConsultationService, MongoConsultationService>();
                    var connectionString = "Host=localhost;Port=15432;Database=prescription_db;Include Error Detail=true;Username=prescription_user;Password=prescription_pw";
                    var host = "localhost";
                    var port = "5432";
                    services.AddSingleton<IPrescriptionRepo>(new DapperPrescriptionRepo(connectionString, host, port));
                    services.AddTransient<Neo4jClient>();
                    services.AddSingleton<IGraphClient>(client);
                }).UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var myService = services.GetRequiredService<Neo4jClient>();
                    var result = await myService.Run();

                    Console.WriteLine($"{string.Join(", ",result)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            
        }


    }
}
