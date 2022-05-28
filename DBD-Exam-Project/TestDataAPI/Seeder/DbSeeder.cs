using Bogus;
using lib.Models;
using lib.Password;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestDataAPI.Context;
using TestDataAPI.DAP;

namespace TestDataAPI.Seeder
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

        IPrescriptionRepo _repo;


        HashSet<string> cprs = new HashSet<string>();
        Random random = new Random();

        static bool testDataGenerated;

        private int addCount = 0;
        private int _divider;

        private PrescriptionContext _prescriptionContext;

        public DbSeeder(PrescriptionContext prescriptionContext, IPrescriptionRepo prescriptionRepo)
        {
            _prescriptionContext = prescriptionContext;
            _repo = prescriptionRepo;
        }

        public void SeedTestData(int divider = 1)
        {
            if (testDataGenerated)
                return;

            testDataGenerated = true;
            _divider = divider;

            var add = CreateAddresses();
            var pharmacies = CreatePharmacies(add);
            var meds = CreateMedicines();
            var patients = CreatePatients(add);
            var pharmaceuts = CreatePharmaceuts(add,pharmacies);
            var doctors = CreateDoctors(add);
            var prescriptions = CreatePrescriptions(meds, doctors, patients);

            try
            {
                _prescriptionContext.AddRange(pharmacies);
                _prescriptionContext.AddRange(meds);
                _prescriptionContext.AddRange(patients);
                _prescriptionContext.AddRange(pharmaceuts);
                _prescriptionContext.AddRange(doctors);
                _prescriptionContext.AddRange(prescriptions);
                _prescriptionContext.Add(CreateTestPatient());
                Console.WriteLine("Save Changes");
                var entries = _prescriptionContext.SaveChanges();
                Console.WriteLine($"Done! - Wrote {entries} entries");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
       

        private List<Prescription> CreatePrescriptions(List<Medicine> meds, List<Doctor> docs, List<Patient> patients)
        {
            Console.WriteLine("Create Prescriptions");
            var random = new Random();

            prescriptionFaker
                .RuleFor(p => p.Creation, (f, p) => f.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now))
                .RuleFor(p => p.Expiration, (f, p) => p.Creation.AddDays(30))
                .RuleFor(p => p.PrescribedByNavigation, (f, p) => docs[random.Next(docs.Count)])
                .RuleFor(p => p.PrescribedToNavigation, (f, p) => patients[random.Next(patients.Count)])
                .RuleFor(p => p.PrescribedToCpr, (f, p) =>
                {
                    Console.WriteLine($"Added prescription for user: {p.PrescribedToNavigation.Cpr} Pw: {p.PrescribedToNavigation.PersonalData.FirstName}");
                    return p.PrescribedToNavigation.Cpr;
                })

        .RuleFor(p => p.Medicine, (f, p) => meds[random.Next(meds.Count)]);

            return prescriptionFaker.Generate(PRESCRIPTION_COUNT);
        }

        private List<Doctor> CreateDoctors(List<Address> add)
        {
            Console.WriteLine("Create Doctors");
            int count = 0;

            doctorFaker
                .RuleFor(d => d.PersonalData, (f, d) => CreatePersonalData("doctor", $"doctor{count++}", "doctor"));

            return doctorFaker.Generate(DOCTOR_COUNT / _divider);
        }

        private List<Pharmaceut> CreatePharmaceuts(List<Address> add, List<Pharmacy> pharmacies)
        {
            var random = new Random();
            Console.WriteLine("Create Pharmaceuts");
            int count = 0;
            pharmaceutFaker
                .RuleFor(p => p.PersonalData, (f, p) => CreatePersonalData("pharmaceut", $"pharmaceut{count++}", "pharmaceut"))
                .RuleFor(p=>p.Pharmacy, (f, p) => pharmacies[random.Next(pharmacies.Count)]);

            return pharmaceutFaker.Generate(PHARMACEUT_COUNT / _divider);
        }

        private List<Patient> CreatePatients(List<Address> add)
        {
            Console.WriteLine("Create Patients");

            List<Patient> patients = new List<Patient>();
            var perIteration = ( PATIENT_COUNT / _divider ) / 100;
            Stopwatch timer = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                timer.Restart();
                Console.WriteLine($"Created{i*perIteration} patients");

                patientFaker
                    .CustomInstantiator(f => new(CreateCpr(f.Person.DateOfBirth.ToString("ddMMyy"))))
                    .RuleFor(p => p.PersonalData, (f, p) => CreatePersonalData("patient", $"{p.Cpr}", "patient"));
                
                patients.AddRange(patientFaker.Generate(perIteration));
                Console.WriteLine($"Patients created in {timer.Elapsed.Seconds} seconds");
            }

            return patients;
        }

        private string CreateCpr(string bDay)
        {
            string cpr = bDay + random.Next(10).ToString() + random.Next(10).ToString() + random.Next(10).ToString() + random.Next(10).ToString();
            while (cprs.Contains(cpr))
            {
                cpr = bDay + random.Next(10).ToString() + random.Next(10).ToString() + random.Next(10).ToString() + random.Next(10).ToString();
            }
            cprs.Add(cpr);
            return cpr;
        }

        private List<Medicine> CreateMedicines()
        {
            Console.WriteLine("Create Medicines");
            var med = new List<Medicine>();

            med.Add(new Medicine("Constaticimol"));
            med.Add(new Medicine("Abracadabrasol"));
            med.Add(new Medicine("Fecetiol"));
            med.Add(new Medicine("Gormanol"));
            med.Add(new Medicine("Crapinal"));
            med.Add(new Medicine("Hamigel"));
            med.Add(new Medicine("Docanital"));
            med.Add(new Medicine("Tyfusal"));
            med.Add(new Medicine("Postgresual"));
            med.Add(new Medicine("Databasimal"));

            return med;
        }

        private List<Pharmacy> CreatePharmacies(List<Address> add)
        {
            Console.WriteLine("Create Pharmacies");
            return pharmacyFaker
                .CustomInstantiator(f =>  new(f.Company.CompanyName()))
                .RuleFor(p => p.Address, (p, f) => add[addCount++])
                .Generate(PHARMACY_COUNT / _divider);
        }

        private List<Address> CreateAddresses()
        {
            Console.WriteLine("Create Addresses");

            return addFaker
                .CustomInstantiator(f => new(f.Address.StreetName(), f.Address.BuildingNumber(), f.Address.ZipCode("####")))
                .Generate(ADDRESS_COUNT / _divider);
        }

        private Patient CreateTestPatient()
        {
            var patient = new Patient("0011223344");
            patient.Prescriptions = new List<Prescription>{
            new Prescription() { MedicineId= 1, PrescribedBy = 1, PrescribedToNavigation = patient, PrescribedToCpr = patient.Cpr, Creation = DateTime.Now}
            };

            patient.PersonalData = CreatePersonalData("testuser", "0011223344", "patient", "testpatient");

            return patient;
        }

        private PersonalDatum CreatePersonalData(string emailPostfix, string username, string role, string firstname = null)
        {
            var personalDataFaker = new Faker<PersonalDatum>()
                    .CustomInstantiator(f => new(firstname ?? f.Name.FirstName(), f.Name.LastName()))
                    .RuleFor(p => p.Email, (f, p) => $"{p.FirstName}@{p.LastName}.emailPostfix")
                    .FinishWith((f, p) => p.Login = CreateLoginInfo(username, p.FirstName.Replace("'", ""), role));
            return personalDataFaker.Generate();
        }

        private LoginInfo CreateLoginInfo(string username, string password, string role)
        {
            var salt = Salt.Create();
            var hash = Hash.Create(password, salt);

            var login = _repo.AddUser(username, hash, salt, password, role);

            LoginInfo loginInfo = _prescriptionContext.LoginInfos.First(x => x.Id == login.Id);

            return loginInfo;
        }
    }
}
