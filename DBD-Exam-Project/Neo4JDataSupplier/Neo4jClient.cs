using Microsoft.Extensions.Logging;
using Neo4jClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using lib.Models;
using System;
using PrescriptionService.Controllers;
using lib.DTO;

namespace Neo4JDataSupplier
{
    public class Neo4jClient
    {
        private readonly ILogger _logger;
        private readonly IGraphClient _client;
        private InfomationController _infomationController;
        

        public InfomationController InfomationController { get => _infomationController; set=>_infomationController=value; }


        public Neo4jClient(IGraphClient client)
        {
            _infomationController = new InfomationController(); 
            _client = client;
        }

        public async Task<string> Run()
        {
            await Prescription();
            await Patient();
            return "Task Completed";
        }

        public async Task<IEnumerable<PatientDto>> Patient()
        {
            var patient = InfomationController.GetPatient();
            foreach (PatientDto item in patient)
            {
                await _client.Cypher.Merge("(p:Patient {Id: $pID })")
                    .OnMatch()
                    .Set("p=$pat")
                    .OnCreate()
                    .Set("p=$pat")
                    .WithParam("pat", item)
                    .WithParam("pID", item.Id)
                    .ExecuteWithoutResultsAsync();
            }



            return patient;
        }
        public async Task<IEnumerable<PrescriptionDto>> Prescription()
        {
            var prescription = InfomationController.GetPrescription();
            foreach (PrescriptionDto item in prescription)
            {
                await _client.Cypher.Merge("(p:Prescription {Id: $pID} )")
                    .OnMatch()
                    .Set("p=$pre")
                    .OnCreate()
                    .Set("p=$pre")
                    .WithParam("pre",item)
                    .WithParam("pID",item.Id)
                    .ExecuteWithoutResultsAsync();
            }
            


            return prescription;
        }

    }
}