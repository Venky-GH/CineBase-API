using CineBaseV2.DatabaseHandler.Interfaces;
using CineBaseV2.Model;
namespace CineBaseV2.DatabaseHandler
{
    using Dapper;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class ProducerDatabaseHandler : IProducerDatabaseHandler
    {
        private string ConnectionString { get; set; }

        public ProducerDatabaseHandler(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("CineBaseConnectionString");
        }

        public IEnumerable<Producer> GetProducers()
        {
            var query = "SELECT * FROM Producer";
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<Producer>(query).ToList();
            }
        }

        public Producer GetProducer(long id)
        {
            var query = "SELECT * FROM Producer WHERE Id = @id";
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.QuerySingle<Producer>(query, new { id });
            }
        }

        public long AddProducer(Producer producer)
        {
            var query = @"INSERT INTO Producer
                        (Name, Sex, DateOfBirth, Biography)
                        OUTPUT INSERTED.Id
                        VALUES
                        (@name, @sex, @dateOfBirth, @biography)";
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<long>(query, new
                {
                    name = producer.Name,
                    sex = producer.Sex,
                    dateOfBirth = producer.DateOfBirth,
                    biography = producer.Biography
                }).First();
            }
        }

        public void DeleteProducer(long id)
        {
            var query = "DELETE FROM Producer WHERE Id = @id";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { id });
            }
        }
    }
}
