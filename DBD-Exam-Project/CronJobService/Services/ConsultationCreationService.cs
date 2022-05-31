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
        private IEnumerable<PersonDto> GetDoctors()
        {
            var doctorClient = new RestClient(_prescriptionServiceUrl);
            var doctorRequest = new RestRequest("api/Persons/doctors?Number=0&Size=10000");
            doctorRequest.AddHeader("content-type", "application/json");
            var doctors = doctorClient.GetAsync<IEnumerable<PersonDto>>(doctorRequest, CancellationToken.None).Result;

            return doctors;
        }

        public void CreateNewConsultationOpenings()
        {
            var doctors = GetDoctors();

            try
            {
                var consultationClient = new RestClient(_consultationServiceUrl);
                var consultationMetadataRequest = new RestRequest("api/ConsultationMetadata");
                consultationMetadataRequest.AddHeader("content-type", "application/json");
                var metadata = consultationClient.GetAsync<ConsultationMetadataDto>(consultationMetadataRequest, CancellationToken.None).Result;
                if (metadata != null && metadata.DayOfConsultationsAdded >= DateTime.Today.AddDays(1) && metadata.CreatedCount > 0)
                {
                    _logger.LogInformation("{0} >= {1} - No consultations will be added", metadata.DayOfConsultationsAdded, DateTime.Today.AddDays(1));
                    return;
                }
                _logger.LogInformation("{0} < {1} Or no consultations were added yet - Consultations will be added", metadata?.DayOfConsultationsAdded, DateTime.Today.AddDays(1));

                int count = 0;
                var time = DateTime.Today.AddDays(1).AddHours(8);
                while (time < DateTime.Today.AddDays(1).AddHours(16))
                {
                    foreach (PersonDto doctor in doctors)
                    {
                        var consultationRequest = new RestRequest("api/Consultation", Method.Post);
                        var consultation = new ConsultationCreationDto()
                        {
                            DoctorId = doctor.Id.ToString(),
                            ConsultationStartUtc = time,
                            GeoPoint = new GeoPointDto(doctor.Address.Longitude, doctor.Address.Latitude)
                        };
                        consultationRequest.AddJsonBody(consultation);
                        consultationRequest.AddHeader("content-type", "application/json");
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
                var consultationMetadataCreateRequest = new RestRequest("api/ConsultationMetadata");
                consultationMetadataCreateRequest.AddJsonBody(metadataToCreate);
                var metadataCreated = consultationClient.PostAsync<ConsultationMetadataDto>(consultationMetadataCreateRequest, CancellationToken.None).Result;
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
