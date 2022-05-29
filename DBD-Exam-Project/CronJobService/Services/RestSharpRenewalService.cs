using lib.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CronJobService.Services
{
    public class RestSharpRenewalService : IRenewalService
    {
        private string _prescriptionServiceEndpoint;
        private string _notificationServiceEndpoint;
        private ILogger<RestSharpRenewalService> _logger;

        public RestSharpRenewalService(IConfiguration config, ILogger<RestSharpRenewalService> logger)
        {
            _prescriptionServiceEndpoint = config["PrescriptionEndpoint"] ?? throw new ArgumentNullException("PrescriptionEndpoint");
            _notificationServiceEndpoint = config["NotificationEndpoint"] ?? throw new ArgumentNullException("NotificationEndpoint");
            _logger = logger;
            _logger.LogInformation($"RenewalService instantiated with endpoints: {_prescriptionServiceEndpoint} {_notificationServiceEndpoint}");
        }

        public void NotifyRenewals()
        {
            _logger.LogInformation($"Notifying about expiring prescriptions");
            CancellationToken cancellationToken = new CancellationToken();
            var prescriptionClient = new RestClient(_prescriptionServiceEndpoint);
            var prescriptionRequest = new RestRequest("Prescription");
            var prescriptionResponse = prescriptionClient.GetAsync<List<PrescriptionDto>>(prescriptionRequest, cancellationToken).Result;

            var notificationClient = new RestClient(_notificationServiceEndpoint);

            try
            {

                foreach (var prescription in prescriptionResponse)
                {
                    var email = prescription.Patient.Email;

                    _logger.LogInformation($"Sending expiration email to {email}");
                    MailRequestDto mailRequest = new MailRequestDto();
                    mailRequest.ToEmail = email;
                    mailRequest.Subject = "Prescription Expiring";
                    mailRequest.Body = $"Your prescription for {prescription.MedicineName} expires {prescription.Expiration}";
                    var notificationRequest = new RestRequest("api/Email/send").AddBody(mailRequest);

                    var response = notificationClient.PostAsync(notificationRequest, cancellationToken).Result;

                    if (!response.IsSuccessful)
                        _logger.LogWarning($"Failed to notify {email}");

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending email notifications", ex);
            }
        }
    }
}