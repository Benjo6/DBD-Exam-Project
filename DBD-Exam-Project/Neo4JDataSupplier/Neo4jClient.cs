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

namespace Neo4JDataSupplier
{
    public class Neo4jClient
    {
        private readonly IGraphClient _client;
        public Neo4jClient(IGraphClient client)
        {
            _client = client;
        }

        public async Task<string> Run()
        {
            //await Prescription();
            //await Patient();
            //await Pharmacies();
            //await Pharamceuts();
            //await Medicines();
            //await Doctors();
            //await Consultations();
            await Prescribed();
            await Prescribed_By();
            await Work_For();
            await Booked();
            await Schedule_For();
            await Prescribed_To();

            return "Task Completed";
        }

        public async Task<IEnumerable<DoctorDto>> Doctors()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("https://localhost:44346/api/doctor");
                IList<DoctorDto> doctors = JsonConvert.DeserializeObject<IList<DoctorDto>>(content);
                foreach (DoctorDto item in doctors)
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

        public async Task<IEnumerable<MedicineDto>> Medicines()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("https://localhost:44346/api/medicine");
                IList<MedicineDto> medicines = JsonConvert.DeserializeObject<IList<MedicineDto>>(content);
                foreach (MedicineDto item in medicines)
                {
                    await _client.Cypher.Merge("(m:Medicine {Id: $mID } )")
                        .OnMatch()
                        .Set("m=$med")
                        .OnCreate()
                        .Set("m=$med")
                        .WithParam("med", item)
                        .WithParam("mID", item.Id)
                        .ExecuteWithoutResultsAsync();
                }



                return medicines;
            }
        }
        public async Task<IEnumerable<PharamceutDto>> Pharamceuts()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("https://localhost:44346/api/pharamceut");
                IList<PharamceutDto> pharamceuts = JsonConvert.DeserializeObject<IList<PharamceutDto>>(content);
                foreach (PharamceutDto item in pharamceuts)
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

        public async Task<IEnumerable<PatientDto>> Patient()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("https://localhost:44346/api/prescription/patient");
                IList<PatientDto> patients = JsonConvert.DeserializeObject<IList<PatientDto>>(content);
                foreach (PatientDto item in patients)
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
                var jsonSerializerSettings = new JsonSerializerSettings();

                string content = await client.GetStringAsync("https://localhost:44346/api/prescription/prescriptions");
                IList<PrescriptionDto> prescriptions = JsonConvert.DeserializeObject<IList<PrescriptionDto>>(content, jsonSerializerSettings);
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
        public async Task<IEnumerable<PharmacyDto>> Pharmacies()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("https://localhost:44346/api/prescription/pharmacy");
                IList<PharmacyDto> pharmacies = JsonConvert.DeserializeObject<IList<PharmacyDto>>(content);
                foreach (PharmacyDto item in pharmacies)
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

        public async Task<string> Prescribed()
        {
            await _client.Cypher.Match("(p:Prescription)", "(m:Medicine)")
                .Where("p.MedicineId = m.Id")
                // .Where((PrescriptionDto p, MedicineDto m) => p.MedicineId == m.Id)
                .Merge("(p)-[:prescribed]->(m)")
                .ExecuteWithoutResultsAsync();

            return "Task Completed for method Prescribed";
        }
        public async Task<string> Prescribed_By()
        {
            await _client.Cypher.Match("(d:Doctor)", "(p:Prescription)")
                .Where("d.Id = p.DoctorId")
             //   .Where((DoctorDto d,PrescriptionDto p ) => d.Id==p.DoctorId)
                .Merge("(d)-[:prescribed_by]->(p)")
                .ExecuteWithoutResultsAsync();

            return "Task Completed for method Prescribed_By";
        }
        public async Task<string> Prescribed_To()
        {
            await _client.Cypher.Match("(p:Prescription)", "(pa:Patient)")
                .Where("p.Id = pa.PatientId")
               // .Where((PatientDto pa, PrescriptionDto p) => pa.Id == p.PatientId)
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
                .Where((PharamceutDto p, PharmacyDto pha) => p.PharmacyName==pha.Name)
                .Merge("(p)-[:work_for]->(pha)")
                .ExecuteWithoutResultsAsync();

            return "Task Completed for method Work_For";
        }

    }
}