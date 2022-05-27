﻿using Microsoft.Extensions.Logging;
using Neo4jClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using lib.Models;
using lib.DTO;
using System.Net.Http;
using Newtonsoft.Json;
using System;
using lib.Converter;
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
            await Prescription();
            await Patient();
            await Pharmacies();
            await Medicines();
            await Doctors();
            //await Consultations();
            return "Task Completed";
        }

        public async Task<IEnumerable<Doctor>> Doctors()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("https://localhost:44346/api/prescription/doctor");
                IList<Doctor> doctors = JsonConvert.DeserializeObject<IList<Doctor>>(content);
                foreach (Doctor item in doctors)
                {
                    await _client.Cypher.Merge("(d:D {Id: $dID } )")
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
            using(HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("https://localhost:44346/api/prescription/medicine");
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
                IList<PrescriptionDto> prescriptions = JsonConvert.DeserializeObject<IList<PrescriptionDto>>(content,jsonSerializerSettings);
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
    }
}