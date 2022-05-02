using System.Data;
using Dapper;
using Npgsql;
using TestDataApi;

namespace PrescriptionService.DAL
{
    public class DapperPrescriptionRepo : IPrescriptionRepo
    {
        private string _connectionsString;

        public DapperPrescriptionRepo(string connectionString)
        {
            _connectionsString = connectionString;
        }

        public LoginInfo AddUser(string username, string password, string salt, string passwordRaw, string role)
        {
            var func = $"select prescriptions.create_{role}(@username::varchar, @password_hashed::varchar, @password_salt::varchar, @password_raw::varchar)";

            var param = new DynamicParameters();
                param.Add("@username", username);
                param.Add("@password_hashed", password);
                param.Add("@password_salt", salt);
                param.Add("@password_raw", passwordRaw);
            using (var connection = new NpgsqlConnection(_connectionsString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                var result = connection.Query(
            sql: func,
            param: param,
            commandType: CommandType.Text
            );

            Console.WriteLine($"User: {username} - PW: {passwordRaw}");

            return GetUserByUsername(username);
        }
        }

        public LoginInfo GetUserByUsername(string username)
        {
            using (var connection = new NpgsqlConnection(_connectionsString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;


                var query =
                    @$"
                        SELECT * FROM prescriptions.login_info login
                        WHERE login.username like @name::varchar                        
                    ";

                var param = new DynamicParameters();
                param.Add("@name", username);

                
                return connection.QuerySingle<LoginInfo>(query, param: param);
                

            }
        }
    }
}
