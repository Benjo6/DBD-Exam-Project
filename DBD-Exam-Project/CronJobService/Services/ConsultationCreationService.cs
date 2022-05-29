using lib.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CronJobService.Services
{
    public class ConsultationCreationService : IConsultationCreationService
    {
        private ILogger<ConsultationCreationService> _logger;
        private IConfiguration _config;

        private string _prescriptionServiceUrl;
        private string _consultationServiceUrl;

        public ConsultationCreationService(ILogger<ConsultationCreationService> logger, IConfiguration config)
        {
            _logger = logger;
            _prescriptionServiceUrl = config["PrescriptionServiceUrl"];
            _consultationServiceUrl = config["ConsultationServiceUrl"];

            _logger.LogInformation("{0}:{1}", nameof(_prescriptionServiceUrl), _prescriptionServiceUrl);
            _logger.LogInformation("{0}:{1}", nameof(_consultationServiceUrl), _consultationServiceUrl);

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

            try
            {

                var consultationClient = new RestClient(_consultationServiceUrl);
                var consultationMetadataRequest = new RestRequest("api/ConsultationMetadata");
                var metadata = consultationClient.GetAsync<ConsultationMetadataDto>(consultationMetadataRequest, CancellationToken.None).Result;
                if (metadata != null && metadata.DayOfConsultationsAdded >= DateTime.Today.AddDays(1))
                {
                    _logger.LogInformation("{0} >= {1} - No consultations will be added", metadata.DayOfConsultationsAdded, DateTime.Today.AddDays(1));
                    return;
                }
                _logger.LogInformation("{0} < {1} - Consultations will be added", metadata?.DayOfConsultationsAdded, DateTime.Today.AddDays(1));

                int count = 0;
                var time = DateTime.Today.AddDays(1).AddHours(8);
                while (time < DateTime.Today.AddDays(1).AddHours(16))
                {
                    // TODO update when doctor controller is up
                    for (int i = 1; i <= 10; i++)
                    {
                        var consultationRequest = new RestRequest("api/Consultation", Method.Post);
                        var consultation = new ConsultationCreationDto()
                        {
                            DoctorId = i.ToString(),
                            ConsultationStartUtc = time,
                            GeoPoint = geoPointDtos[i % geoPointDtos.Count]
                        };
                        consultationRequest.AddJsonBody(consultation);
                        var consultationResponse = consultationClient.PostAsync(consultationRequest, CancellationToken.None).Result;
                        if (!consultationResponse.IsSuccessful)
                        {
                            _logger.LogWarning("Error response while attempting to create consultation: {0}", consultationResponse.StatusCode);
                            break;
                        }
                        count++;
                    }
                    time = time.AddMinutes(20);
                }
                var metadataToCreate = new ConsultationMetadataDto()
                {
                    DayOfConsultationsAdded = DateTime.Today.AddDays(1),
                    CreatedCount = count,
                    CreatedUtc = DateTime.UtcNow
                };
                var consultationMetadataCreRequest = new RestRequest("api/ConsultationMetadata");
                var metadataCreated = consultationClient.PostAsync<ConsultationMetadataDto>(consultationMetadataCreRequest, CancellationToken.None).Result;
                _logger.LogInformation("{0}\ncreated", JsonConvert.SerializeObject(metadataCreated, Formatting.Indented));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ConsultationCreationJob:");
                throw;
            }
        }
    }
}
