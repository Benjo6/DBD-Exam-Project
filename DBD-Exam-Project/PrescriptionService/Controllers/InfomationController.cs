using lib.DTO;
using lib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrescriptionService.DAP;
using System.Collections.Generic;

namespace PrescriptionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfomationController : ControllerBase
    {
        public InfomationController( )
        {

        }

        [HttpGet(Name="Prescription")]
        public IEnumerable<PrescriptionDto> GetPrescription()
        {
            List<PrescriptionDto> prescriptionlist = new List<PrescriptionDto>();
            prescriptionlist.Add(new PrescriptionDto { Creation = System.DateTime.Now, Expiration = System.DateTime.Today, Id = 1 });

            var result = prescriptionlist;
            return result;
        }
        [HttpGet(Name = "Patient")]
        public IEnumerable<PatientDto> GetPatient()
        {
            List<PatientDto> patientlist = new List<PatientDto>();
            patientlist.Add(new PatientDto { Id=1, FirstName="Benjo",LastName="Churovich", Email="NoEmail@for.you" });
            patientlist.Add(new PatientDto { Id = 2, FirstName = "Tihomir", LastName = "Stojkovic", Email = "Stojko@for.you" });

            var result = patientlist;
            return result;
        }



    }
}
