using lib.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CronJobService.Services
{
    public class ConsultationCreationService : IConsultationCreationService
    {
        private ILogger<ConsultationCreationService> _logger;
        private IConfiguration _config;

        private string _mongoConnectionString;
        private string _prescriptionServiceUrl;
        private string _consultationServiceUrl;

        public ConsultationCreationService(ILogger<ConsultationCreationService> logger, IConfiguration config)
        {
            _logger = logger;
            _mongoConnectionString = config.GetConnectionString("mongodb");
            _prescriptionServiceUrl = config["PrescriptionServiceUrl"];
            _consultationServiceUrl = config["ConsultationServiceUrl"];

        }
        public void CreateNewConsultationOpenings()
        {
            // Coordinates:
            // Kalundborg 55,6783 11,1084
            // Holbæk 55,7077 11,7113
            // Ringsted 55,4508 11,7937
            // Hillerød 55,9314 12,2990
            // Nørrebro 55,6973 12,5542
            // Hvidovre 55,6413 12,4763

            List<GeoPointDto> geoPointDtos = new List<GeoPointDto>() { 
                new GeoPointDto(55.6783, 11.1084),
                new GeoPointDto(55.7077, 11.7113),
                new GeoPointDto(55.4508, 11.7937),
                new GeoPointDto(55.9314, 12.2990),
                new GeoPointDto(55.6973, 12.5542),
                new GeoPointDto(55.6413, 12.4763)
            };

            var consultationClient = new RestClient(_consultationServiceUrl);
            
            var time = DateTime.Today.AddDays(1).AddHours(8);
            while(time < DateTime.Today.AddDays(1).AddHours(16))
            {
                // TODO update when doctor controller is up
                for (int i = 1; i <= 10; i++)
                {
                    var consultationRequest = new RestRequest("Consultations");
                    var consultation = new ConsultationCreationDto() {
                        DoctorId = i.ToString(),
                        ConsultationStartUtc = time,
                        GeoPoint = geoPointDtos[i % geoPointDtos.Count]
                    };
                    consultationRequest.AddJsonBody(consultation);
                    var consultationResponse = consultationClient.PostAsync(consultationRequest, CancellationToken.None).Result;
                }
                time = time.AddMinutes(20);
            }
            
        }
    }
}
