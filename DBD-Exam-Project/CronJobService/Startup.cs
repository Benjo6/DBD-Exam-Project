using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using CronJobService.Service;
using System;
using CronJobService.Services;

namespace CronJobService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCronJob<RenewalJob>(renewal =>
            {
                renewal.CronExpression = @"*/15 * * * *";
                renewal.TimeZoneInfo = TimeZoneInfo.Utc;
            });

            services.AddCronJob<ConsultationJob>(consultationJob =>
            {
                consultationJob.CronExpression = @"* * * * *";
                consultationJob.TimeZoneInfo = TimeZoneInfo.Utc;
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CronJobService", Version = "v1" });
            });
            services.AddSingleton<IRenewalService, RestSharpRenewalService>();
            services.AddSingleton<IConsultationCreationService, ConsultationCreationService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CronJobService v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
