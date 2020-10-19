using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace ShoppingDB.Services
{
    public interface IDbConnection
    {
        NpgsqlConnection CreateDbConnection();
    }
}
