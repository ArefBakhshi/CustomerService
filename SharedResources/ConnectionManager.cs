using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;


namespace SharedResources
{
    public static class ConnectionManager
    {
        public static string GetConnectionString()
        {

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;
            return connectionString;
        }
    }
}

