using Microsoft.Extensions.Logging;
using Neo4jClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using lib.Models;
using System;
using PrescriptionService.Controllers;
using lib.DTO;
using PrescriptionService.DAP;

namespace Neo4JDataSupplier
{
    public class Neo4jClient
    {
        private readonly IGraphClient _client;
        private PrescriptionController _prescriptionController;
        private readonly IPrescriptionRepo _repo;
        

        public PrescriptionController PrescriptionController { get => _prescriptionController; set=> _prescriptionController = value; }


        public Neo4jClient(IGraphClient client, IPrescriptionRepo repo)
        {
            _repo = repo;
            _prescriptionController = new PrescriptionController(repo); 
            _client = client;
        }

        public async Task<string> Run()
        {
            await Prescription();
            await Patient();
            return "Task Completed";
        }

        public async Task<IEnumerable<Patient>> Patient()
        {
            var patient = PrescriptionController.GetAllPatients();
            foreach (Patient item in patient)
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
            var prescription = PrescriptionController.GetAllPrescriptions();
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