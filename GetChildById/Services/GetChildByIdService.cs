using Oracle.ManagedDataAccess.Client;
using System.Data;
using GetAdultById.Models;

namespace GetChildById.Services
{
    public class GetChildByIdService
    {
        private readonly IConfiguration _config;

        public GetChildByIdService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Child?> GetChildByIdAsync(int id)
        {
            using var conn = new OracleConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Name, Lastname, BirthYear, ImageUrl FROM CHILD WHERE Id = :id";
            cmd.Parameters.Add(new OracleParameter("id", id));

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Child
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Lastname = reader.GetString(2),
                    BirthYear = reader.GetInt32(3),
                    ImageUrl = reader.GetString(4)
                };
            }

            return null;
        }
    }
}
