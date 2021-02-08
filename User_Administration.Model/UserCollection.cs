using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace User_Administration.Model
{
    public interface IUserRepository
    {
        int CountUsers(string username, string password);
        IEnumerable<User> GetAll();
    }
    public class UserCollection : Repository, IUserRepository
    {

        public int CountUsers(string username, string password)
        {
            const string sql = @"SELECT COUNT(*)
                                FROM dbo.Users
                                WHERE Users.UsernAME = @Username
                                  AND Users.uSERpASS = @Password";

            using var conn = Connection;

            return conn.QueryFirst<int>(sql, new
            {
                Username = username,
                Password = password
            });
        }

        public  IEnumerable<User> GetAll()
        {
            const string sql = @"SELECT * FROM dbo.Users";

            using var conn = Connection;

            return conn.Query<User>(sql);
        }
    }
}
