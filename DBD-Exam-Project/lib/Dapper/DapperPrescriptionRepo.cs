using Dapper;
using lib.Interfaces;
using lib.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.Dap
{
    public class DapperPrescriptionRepo : IPrescriptionRepo
    {
        private string _connectionString;

        public DapperPrescriptionRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public LoginInfo AddUser(string username, string password, string salt, string passwordRaw, string role)
        {
            var func = $"select prescriptions.create_{role}(@username::varchar, @password_hashed::varchar, @password_salt::varchar, @password_raw::varchar)";

            var param = new DynamicParameters();
            param.Add("username", username);
            param.Add("@password_hashed", password);
            param.Add("@password_salt", salt);
            param.Add("@password_raw", passwordRaw);

            using(var connection = new NpgsqlConnection(_connectionString))
            {
               DefaultTypeMap.MatchNamesWithUnderscores = true;
                var result = connection.Query(
                    sql: func,
                    param: param,
                    commandType: CommandType.Text);
                Console.WriteLine($"User: {username} - PW: {passwordRaw}");

                return GetUserByUsername(username);
            }
        }

        public LoginInfo GetUserByUsername(string username)
        {
            using(var connect = new NpgsqlConnection(_connectionString))
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;

                var query =
                    @$"SELECT * FROM prescriptions.login_info login
                        WHERE login.username like @name::varchar";

                var param = new DynamicParameters();
                param.Add("@name", username);

                return connect.QuerySingle<LoginInfo>(query, param: param);
            }
        }
    }
}
