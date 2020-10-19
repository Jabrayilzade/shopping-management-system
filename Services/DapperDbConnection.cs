using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ShoppingDB.Services
{
    public class DapperDbConnection : IDbConnection
    {
        public DapperDbConnection(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public NpgsqlConnection CreateDbConnection()
        {
            string connectionName = Configuration.GetConnectionString("ShoppingMS");
            using(NpgsqlConnection connection = new NpgsqlConnection(connectionName))
            {
                connection.Open();
                return new NpgsqlConnection(connectionName);
            }
        }

    }
}
