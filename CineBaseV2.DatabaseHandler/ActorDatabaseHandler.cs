namespace CineBaseV2.DatabaseHandler
{
    using CineBaseV2.DatabaseHandler.Interfaces;
    using CineBaseV2.Model;
    using Dapper;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class ActorDatabaseHandler : IActorDatabaseHandler
    {
        private string ConnectionString { get; set; }

        public ActorDatabaseHandler(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("CineBaseConnectionString");
        }

        public IEnumerable<Actor> GetActors()
        {
            var query = "SELECT * FROM Actor";
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<Actor>(query).ToList();
            }
        }

        public Actor GetActor(long id)
        {
            var query = "SELECT * FROM Actor WHERE Id = @id";
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.QuerySingle<Actor>(query, new { id });
            }
        }

        public long AddActor(Actor actor)
        {
            var query = @"INSERT INTO Actor
                        (Name, Sex, DateOfBirth, Biography)
                        OUTPUT INSERTED.Id
                        VALUES
                        (@Name, @Sex, @DateOfBirth, @Biography)";
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.QueryFirst<long>(query, actor);
            }
        }

        public void DeleteActor(long id)
        {
            var query = "DELETE FROM Actor WHERE Id = @id";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { id });
            }
        }
    }
}
