using Microsoft.Extensions.Logging;
using Neo4jClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using lib.Models;
using lib.DTO;
using System.Net.Http;
using Newtonsoft.Json;
using PrescriptionService.DAP;

namespace Neo4JDataSupplier
{
    public class Neo4jClient
    {
        private readonly IGraphClient _client;
        private readonly IPrescriptionRepo _repo;
        public Neo4jClient(IGraphClient client, IPrescriptionRepo repo)
        {
            _repo = repo;
            _client = client;
        }

        public async Task<string> Run()
        {
            //await Prescription();
            await Patient();
            await Pharmacies();
            //await Consultations();
            return "Task Completed";
        }

        public async Task<IEnumerable<Patient>> Patient()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("https://localhost:44346/api/prescription/patient");
                IList<Patient> patients = JsonConvert.DeserializeObject<IList<Patient>>(content);
                foreach (Patient item in patients)
                {
                    await _client.Cypher.Merge("(p:Patient {Id: $pID } )")
                        .OnMatch()
                        .Set("p=$pat")
                        .OnCreate()
                        .Set("p=$pat")
                        .WithParam("pat", item)
                        .WithParam("pID", item.Id)
                        .ExecuteWithoutResultsAsync();
                }



                return patients;
            }
        }
        public async Task<IEnumerable<PrescriptionDto>> Prescription()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("https://localhost:44346/api/prescription/prescriptions");
                IList<PrescriptionDto> prescriptions = JsonConvert.DeserializeObject<IList<PrescriptionDto>>(content);
                foreach (PrescriptionDto item in prescriptions)
                {
                    await _client.Cypher.Merge("(p:Prescription {Id: $pID} )")
                        .OnMatch()
                        .Set("p=$pre")
                        .OnCreate()
                        .Set("p=$pre")
                        .WithParam("pre", item)
                        .WithParam("pID", item.Id)
                        .ExecuteWithoutResultsAsync();
                }



                return prescriptions;
            }
        }
        public async Task<IEnumerable<Pharmacy>> Pharmacies()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("https://localhost:44346/api/prescription/pharmacy");
                IList<Pharmacy> pharmacies = JsonConvert.DeserializeObject<IList<Pharmacy>>(content);
                foreach (Pharmacy item in pharmacies)
                {
                    await _client.Cypher.Merge("(p:Pharmacy {Id: $pID} )")
                        .OnMatch()
                        .Set("p=$pha")
                        .OnCreate()
                        .Set("p=$pha")
                        .WithParam("pha", item)
                        .WithParam("pID", item.Id)
                        .ExecuteWithoutResultsAsync();
                }



                return pharmacies;
            }
        }
        public async Task<IEnumerable<ConsultationDto>> Consultations()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("http://localhost:18080/api/Consultation");
                IList<ConsultationDto> consultations = JsonConvert.DeserializeObject<IList<ConsultationDto>>(content);
                foreach (ConsultationDto item in consultations)
                {
                    await _client.Cypher.Merge("(c:Consultation {Id: $cID} )")
                        .OnMatch()
                        .Set("c=$con")
                        .OnCreate()
                        .Set("c=$con")
                        .WithParam("con", item)
                        .WithParam("cID", item.Id)
                        .ExecuteWithoutResultsAsync();
                }

                return consultations;
            }
        } 
    }
}