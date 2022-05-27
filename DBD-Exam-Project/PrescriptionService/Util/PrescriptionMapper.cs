using lib.DTO;
using lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrescriptionService.Util
{
    public class DtoMapper
    {
        public static PrescriptionDto ToDto(Prescription prescription)
            => new PrescriptionDto
            {
                Id = prescription.Id,
                Creation = prescription.Creation,
                Expiration = prescription.Expiration,
                Patient = prescription.PrescribedToNavigation != null ? new PatientDto
                {
                    FirstName = prescription.PrescribedToNavigation.PersonalData.FirstName,
                    LastName = prescription.PrescribedToNavigation.PersonalData.LastName,
                    Email = prescription.PrescribedToNavigation.PersonalData.Email ?? ""
                } : null,
                Medicine = prescription.Medicine != null ? new MedicineDto {
                    Name = prescription.Medicine.Name
                } : null
            };

        public static PatientDto ToDto(Patient patient)
            => new PatientDto
            {
                Email = patient.PersonalData.Email ?? "",
                FirstName = patient.PersonalData.FirstName,
                LastName = patient.PersonalData.LastName,
                Id = patient.Id
            };
        public static DoctorDto ToDto(Doctor doctor)
           => new DoctorDto
           {
               Email = doctor.PersonalData.Email ?? "",
               FirstName = doctor.PersonalData.FirstName,
               LastName = doctor.PersonalData.LastName,
               Id = doctor.Id
           };


        public static MedicineDto ToDto(Medicine medicine)
            => new MedicineDto
            {
                Id = medicine.Id,
                Name = medicine.Name
            };

        public static PharmacyDto ToDto(Pharmacy pharmacy)
            => new PharmacyDto
            {
                Id = pharmacy.Id,
                Name = pharmacy.PharmacyName,
                Address = pharmacy.Address != null ? new AddressDto
                {
                    Id = pharmacy.Address.Id,
                    StreetName = pharmacy.Address.StreetName,
                    StreetNumber = pharmacy.Address.StreetNumber,
                    ZipCode = pharmacy.Address.ZipCode
                } : new(),
            };
    }
}
