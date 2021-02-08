using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_Administration.Model
{
    public class Repository
    {
        public IDbConnection Connection => new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ToString());
    }
}
