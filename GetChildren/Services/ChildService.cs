using GetChildren.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GetChildren.Services
{
    public class ChildService : IChildService
    {
        private readonly IConfiguration _config;

        public ChildService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Child>> GetAllChildrenAsync()
        {
            var children = new List<Child>();
            using var conn = new OracleConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using var cmd = new OracleCommand("SELECT Id, Name, Lastname, BirthYear, ImageUrl FROM Child", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                children.Add(new Child
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Lastname = reader.GetString(2),
                    BirthYear = reader.GetInt32(3),
                    ImageUrl = reader.GetString(4)
                });
            }

            return children;
        }
    }
}
