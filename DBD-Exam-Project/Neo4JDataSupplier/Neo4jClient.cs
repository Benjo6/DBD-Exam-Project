using Microsoft.Extensions.Logging;
using Neo4jClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using lib.Models;
using lib.DTO;
using System.Net.Http;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Neo4JDataSupplier.Model;

namespace Neo4JDataSupplier
{
    public class Neo4jClient
    {
        private readonly IConfiguration _config;
        private readonly IGraphClient _client;
        public Neo4jClient(IGraphClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task<string> Run()
        {
            //await Patient();
            //await Pharmacies();
            //await Prescription();
            await Pharamceuts();
            //await Medicines();
            //await Doctors();
            //await Consultations();
            await Prescribed();
            await Prescribed_By();
            await Work_For();
            //await Booked();
            //await Schedule_For();
            await Prescribed_To();

            return "Task Completed";
        }

        public async Task<IEnumerable<PersonDto>> Doctors()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(_config.GetValue<string>("ConnectionStrings:prescription_service") + "Persons/doctors?Size=1000");
                IList<PersonDto> doctors = JsonConvert.DeserializeObject<IList<PersonDto>>(content);
                foreach (PersonDto item in doctors)
                {
                    await _client.Cypher.Merge("(d:Doctor {Id: $dID } )")
                        .OnMatch()
                        .Set("d=$doc")
                        .OnCreate()
                        .Set("d=$doc")
                        .WithParam("doc", item)
                        .WithParam("dID", item.Id)
                        .ExecuteWithoutResultsAsync();
                }



                return doctors;
            }
        }

        public async Task<IEnumerable<string>> Medicines()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(_config.GetValue<string>("ConnectionStrings:prescription_service") + "Medicines");
                IList<string> medicines = JsonConvert.DeserializeObject<IList<string>>(content);
                foreach (string item in medicines)
                {
                    await _client.Cypher.Merge("(m:Medicine {Name: $med } )")
                        .WithParam("med", item)
                        .ExecuteWithoutResultsAsync();
                }



                return medicines;
            }
        }
        public async Task<IEnumerable<PersonDto>> Pharamceuts()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(_config.GetValue<string>("ConnectionStrings:prescription_service") + "Persons/pharmaceuts?Size=999");
                IList<PersonDto> pharamceuts = JsonConvert.DeserializeObject<IList<PersonDto>>(content);
                foreach (PersonDto item in pharamceuts)
                {
                    await _client.Cypher.Merge("(p:Pharamceut {Id: $pID } )")
                        .OnMatch()
                        .Set("p=$phar")
                        .OnCreate()
                        .Set("p=$phar")
                        .WithParam("phar", item)
                        .WithParam("pID", item.Id)
                        .ExecuteWithoutResultsAsync();
                }



                return pharamceuts;
            }
        }

        public async Task<IEnumerable<PersonDto>> Patient()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(_config.GetValue<string>("ConnectionStrings:prescription_service") + "Persons/patients?Size=10001");
                IList<PersonDto> patients = JsonConvert.DeserializeObject<IList<PersonDto>>(content);
                foreach (PersonDto item in patients)
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
                string content = await client.GetStringAsync(_config.GetValue<string>("ConnectionStrings:prescription_service") + "Prescriptions?Size=20001");
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
        public async Task<IEnumerable<PharmacyNDto>> Pharmacies()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(_config.GetValue<string>("ConnectionStrings:prescription_service") + "Pharmacy/pharmacy?Size=100");
                IList<PharmacyNDto> pharmacies = JsonConvert.DeserializeObject<IList<PharmacyNDto>>(content);
                foreach (PharmacyNDto item in pharmacies)
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
                string content = await client.GetStringAsync(_config.GetValue<string>("ConnectionStrings:consultation_service") + "Consultation");
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

        public async Task<string> Prescribed()
        {
            await _client.Cypher.Match("(p:Prescription)", "(m:Medicine)")
                .Where((PrescriptionDto p, MedicineDto m) => p.MedicineName == m.Name)
                .Merge("(p)-[:prescribed]->(m)")
                .ExecuteWithoutResultsAsync();

            return "Task Completed for method Prescribed";
        }
        public async Task<string> Prescribed_By()
        {
            await _client.Cypher.Match("(d:Doctor)", "(p:Prescription)")
                .Where((PersonDto d, PrescriptionDto p) => d.Id == p.DoctorId)
                .Merge("(d)-[:prescribed_by]->(p)")
                .ExecuteWithoutResultsAsync();

            return "Task Completed for method Prescribed_By";
        }
        public async Task<string> Prescribed_To()
        {
            await _client.Cypher.Match("(p:Prescription)", "(pa:Patient)")
                .Where((PersonDto pa, PrescriptionDto p) => pa.Id == p.PatientId)
                .Merge("(p)-[:prescribed_to]->(pa)")
                .ExecuteWithoutResultsAsync();

            return "Task Completed for method Prescribed_To";
        }
        public async Task<string> Schedule_For()
        {
            await _client.Cypher.Match("(c:Consultation)", "(d:Doctor)")
                .Where("c.DoctorId = d.Id")
                .Merge("(c)-[:schedule_For]->(d)")
                .ExecuteWithoutResultsAsync();

            return "Task Completed for method Schedule_For";
        }
        public async Task<string> Booked()
        {
            await _client.Cypher.Match("(c:Consultation)", "(p:Patient)")
                .Where("c.PatientId = p.Id")
                .Merge("(p)-[:booked]->(c)")
                .ExecuteWithoutResultsAsync();

            return "Task Completed for method Booked";
        }
        public async Task<string> Work_For()
        {
            await _client.Cypher.Match("(pha:Pharmacy)", "(p:Pharamceut)")
                .Where("p.PharmacyName = pha.Name")
                .Merge("(p)-[:work_for]->(pha)")
                .ExecuteWithoutResultsAsync();

            return "Task Completed for method Work_For";
        }

    }
}