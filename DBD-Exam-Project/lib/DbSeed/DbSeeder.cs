using Bogus;
using lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.DbSeed
{
    public class DbSeeder
    {
        const int ADDRESS_COUNT = 10000;
        const int PHARMACY_COUNT = 100;
        const int DOCTOR_COUNT = 1000;
        const int PATIENT_COUNT = 10000;
        const int PHARMACEUT_COUNT = 1000;
        const int PRESCRIPTION_COUNT = 20000;

        static Faker<Address> addFaker = new Faker<Address>();
        static Faker<Pharmacy> pharmacyFaker = new Faker<Pharmacy>();
        static Faker<Doctor> doctorFaker = new Faker<Doctor>();
        static Faker<Patient> patientFaker = new Faker<Patient>();
        static Faker<Pharmaceut> pharmaceutFaker = new Faker<Pharmaceut>();
        static Faker<Prescription> prescriptionFaker = new Faker<Prescription>();
    }
}
