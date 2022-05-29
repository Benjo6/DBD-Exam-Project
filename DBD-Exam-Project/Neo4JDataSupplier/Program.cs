using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neo4jClient;

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
                    
                })
                .UseConsoleLifetime();
            
          
       

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
