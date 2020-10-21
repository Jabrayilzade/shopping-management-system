using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace ShoppingDB.Services
{
    public class DapperDbConnection : IDbConnectionFactory
    {
        public DapperDbConnection(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IDbConnection CreateDbConnection()
        {
            string connectionName = Configuration.GetConnectionString("ShoppingMS");
            IDbConnection connection = new NpgsqlConnection(connectionName);
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            connection.Open();
            return connection;
        }
    }
}
