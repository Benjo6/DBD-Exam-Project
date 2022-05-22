using lib.DTO;
using lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrescriptionService.Util
{
    public class PrescriptionMapper
    {
        public static PrescriptionDto ToDto(PrescriptionOut prescription)
        {
            var dto = new PrescriptionDto();

            dto.Creation = prescription.Creation;
            dto.Expiration = prescription.Expiration;
            dto.Patient = new PatientDto();
            dto.Patient.FirstName = prescription.PrescribedToNavigation?.PersonalData?.FirstName;
            dto.Patient.LastName = prescription.PrescribedToNavigation?.PersonalData?.LastName;
            dto.Patient.Email = prescription.PrescribedToNavigation?.PersonalData?.Email;
            dto.Medicine = new MedicineDto();
            dto.Medicine.Name = prescription.Medicine?.Name;

            return dto;
        }
    }
}
