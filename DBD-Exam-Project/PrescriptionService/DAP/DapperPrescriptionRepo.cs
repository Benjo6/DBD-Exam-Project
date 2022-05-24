using Dapper;
using lib.DTO;
using lib.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PrescriptionService.DAP
{
    public class DapperPrescriptionRepo : IPrescriptionRepo
    {
        private readonly string _adminConnectionString;
        private readonly string _customConenctionString;

        public DapperPrescriptionRepo(string adminConnectionString, string customConenctionString)
        {
            _adminConnectionString = adminConnectionString;
            _customConenctionString = customConenctionString;
        }

        public async Task<Prescription> CreatePrescription(Prescription prescription)
        {
            var query = @$"INSERT INTO prescriptions.prescription(expiration,expiration_warning_sent,creation,medicine_id,prescribed_by,prescribed_to_cpr,last_administered_by)
                                                                   VALUES(@expiration,@expiration_warning_sent,@creation,@medicine_id,@prescribed_by,@prescribed_to_cpr,@last_administered_by)";
            var param = new DynamicParameters();
            param.Add("@expiration", prescription.Expiration);
            param.Add("@expiration_warning_sent", prescription.ExpirationWarningSent);
            param.Add("@creation", DateTime.Now);
            param.Add("@medicine_id", prescription.MedicineId);
            param.Add("@prescribed_by", prescription.PrescribedBy);
            param.Add("@prescribed_to_cpr", prescription.PrescribedToCpr);
            param.Add("@last_administered_by", prescription.LastAdministeredBy);


            using (var connection = new NpgsqlConnection(_connectionString))
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;

                var result = connection.Query(sql: query, param: param, commandType: CommandType.Text);
            };
            Console.WriteLine($"Id:{prescription.Id}");

            return prescription;
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            using (var connection = new NpgsqlConnection(_adminConnectionString))
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;

                var query = @$" SELECT * FROM prescriptions.patient;";

                var resultList = connection.Query<Patient>(query);

                return resultList;


            }

        }

        public  IEnumerable<Pharmacy> GetAllPharmacies()
        {
            using (var connection = new NpgsqlConnection(_adminConnectionString))
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;


                var query = @$" SELECT id, pharmacy_name, address_id FROM prescriptions.pharmacy;";

                var resultList = connection.Query<Pharmacy>(query);


                return resultList;


            }
        }

        public IEnumerable<Prescription>GetAllPrescriptions()
        {
            using (var connection = new NpgsqlConnection(_adminConnectionString))
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;


                var query = @$" SELECT * FROM prescriptions.prescription;";

                var resultList = connection.Query<Prescription>(query);
                return resultList;


            }

        }

        

        public IEnumerable<Prescription> GetPrescriptionsExpiringLatest(DateOnly expiringDate)
        {
            using (var connection = new NpgsqlConnection(_adminConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                var lookup = new Dictionary<long, Prescription>();

                var query =
                    @$"
                        SELECT
                        pr.id, pr.expiration, pr.creation, pr.medicine_id, pr.prescribed_to, pr.expiration_warning_sent,
                        pat.id, pat.personal_data_id,
                        med.id, med.name,
                        dat.id, dat.email, dat.first_name, dat.last_name
                        
                        FROM prescriptions.prescription pr
                            INNER JOIN prescriptions.patient pat ON pr.prescribed_to = pat.id
                            INNER JOIN prescriptions.personal_data dat ON pat.personal_data_id = dat.id
                            INNER JOIN prescriptions.medicine med ON pr.medicine_id = med.id
                        WHERE pr.expiration < @exp::date AND pr.expiration_warning_sent != true
                        
                    ";

                var param = new DynamicParameters();
                param.Add("@exp", expiringDate.ToString("yyyy-MM-dd"));


                connection.Query<Prescription, Patient, Medicine, PersonalDatum, Prescription>(query, (pr, pat, med, dat) => {
                    Prescription prescription;
                    if (!lookup.TryGetValue(pr.Id, out prescription))
                        lookup.Add(pr.Id, prescription = pr);

                    prescription.PrescribedToNavigation = pat;
                    prescription.Medicine = med;
                    pat.PersonalData = dat;
                    return prescription;
                }, splitOn: "id, id, id, id", param: param).AsQueryable();
                var resultList = lookup.Values;



                return resultList;


            }
        }

        public IEnumerable<Prescription> GetPrescriptionsForUser(string username, string password)
        {
            Console.WriteLine($"Get prescriptions for {username.Substring(0,6)}-xxxx");

            string connString = string.Format(_customConenctionString, username, password);
            using (var connection = new NpgsqlConnection(connString))
            {
                
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                var lookup = new Dictionary<long, Prescription>();

                var query =
                    @$"
                        SELECT
                        pr.id, pr.expiration, pr.creation, pr.medicine_id, pr.prescribed_to, pr.expiration_warning_sent,
                        med.id, med.name
                        
                        FROM prescriptions.prescription pr
                            INNER JOIN prescriptions.medicine med ON pr.medicine_id = med.id
                        WHERE pr.prescribed_to_cpr like @cpr::varchar
                        
                    "
                     ;

                var param = new DynamicParameters();
                param.Add("@cpr", username);


                connection.Query<Prescription, Medicine, Prescription>(query, (pr, med) => {
                    Prescription prescription;
                    if (!lookup.TryGetValue(pr.Id, out prescription))
                        lookup.Add(pr.Id, prescription = pr);

                    prescription.Medicine = med;
                    return prescription;
                }, splitOn: "id, id", param: param).AsQueryable();
                var resultList = lookup.Values;

                return resultList;
            }
        }

        public bool MarkPrescriptionWarningSent(long prescriptionId)
        {
            Console.WriteLine($"Mark prescription with id {prescriptionId} as warned about expiration");
            using (var connection = new NpgsqlConnection(_adminConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                var lookup = new Dictionary<long, Prescription>();

                var query =
                    @$"
                        UPDATE prescriptions.prescription SET expiration_warning_sent = true
                        WHERE id = @id::bigint                  
                    ";

                var param = new DynamicParameters();
                param.Add("@id", prescriptionId);

                connection.Query(query);
                return true;
            }
        }
    }
}
