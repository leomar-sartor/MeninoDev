using Dapper;
using MeninoDev.Entidades;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace MeninoDev.Services
{
    public class CategoriaService : ICategoriaService
    {
        public IConfiguration _configuration { get; }
        public string _connectionString { get; }
        public CategoriaService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("database");
        }
        public IEnumerable<Categoria> BuscarTodas()
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Category";

                return db.Query<Categoria>(sql);
            }
        }
    }
}
