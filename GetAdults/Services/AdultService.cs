using GetAdults.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GetAdults.Services
{
    public class AdultService : IAdultService
    {
        private readonly IConfiguration _config;

        public AdultService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Adult>> GetAllAdultsAsync()
        {
            var adults = new List<Adult>();
            using var conn = new OracleConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using var cmd = new OracleCommand("SELECT Id, Name, Lastname, BirthYear, ImageUrl FROM Adult", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                adults.Add(new Adult
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Lastname = reader.GetString(2),
                    BirthYear = reader.GetInt32(3),
                    ImageUrl = reader.GetString(4)
                });
            }

            return adults;
        }
    }
}
