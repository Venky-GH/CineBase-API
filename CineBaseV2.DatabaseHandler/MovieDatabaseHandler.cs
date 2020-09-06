namespace CineBaseV2.DatabaseHandler
{
    using CineBaseV2.DatabaseHandler.Interfaces;
    using CineBaseV2.Model;
    using Dapper;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class MovieDatabaseHandler : IMovieDatabaseHandler
    {
        public string ConnectionString { get; set; }

        public MovieDatabaseHandler(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("CineBaseConnectionString");
        }

        public IEnumerable<MovieAggregate> GetMovieAggregates(long id = -1)
        {
            var query = @"
                        Select 
                        M.Id, M.Name as MovieName, M.Plot, M.YearOfRelease, M.Image, M.ProducerId,
                        A.Id as ActorId, A.Biography as ActorBiography, A.DateOfBirth as ActorDateOfBirth, A.Sex as ActorSex, A.Name as ActorName,
                        P.Biography as ProducerBiography, P.DateOfBirth as ProducerDateOfBirth, P.Sex as ProducerSex, P.Name as ProducerName
                        from Movie M
                        inner join Producer P
                        on M.ProducerId = P.Id
                        inner join ActorMovieMapping AM
                        on M.Id = AM.MovieId
                        inner join Actor A
                        on A.Id = AM.ActorId";
            if (id != -1)
                query += @" WHERE M.Id = @id";

            using (var connection = new SqlConnection(ConnectionString))
            {
                if (id == -1)
                    return connection.Query<MovieAggregate>(query);
                else
                    return connection.Query<MovieAggregate>(query, new { id });
            }
        }

        public void UpdateMovie(Movie movie, List<long> actorIds)
        {
            var updateMovieQuery = @"
                                    UPDATE Movie
                                    SET Name = @Name,
                                    Image = @Image,
                                    YearOfRelease = @YearOfRelease,
                                    Plot = @Plot,
                                    ProducerId = @ProducerId
                                    WHERE Id = @Id";
            var mapperQuery = @"DELETE FROM ActorMovieMapping WHERE MovieId = @Id";

            foreach(var actorId in actorIds)
            {
                mapperQuery += $@"
                                INSERT INTO ActorMovieMapping
                                (ActorId, MovieId)
                                VALUES
                                ({actorId}, {movie.Id})";
            }

            using(var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(updateMovieQuery, movie);
                connection.Execute(mapperQuery, new { movie.Id });
            }
        }

        public long AddMovie(Movie movie, List<long> actorIds)
        {
            var insertMovieQuery = @"
                                    INSERT INTO Movie
                                    (Name, Image, Plot, YearOfRelease, ProducerId)
                                    OUTPUT INSERTED.Id
                                    VALUES
                                    (@Name, @Image, @Plot, @YearOfRelease, @ProducerId)";

            var mapperQuery = string.Empty;

            foreach (var actorId in actorIds)
            {
                mapperQuery += $@"
                                INSERT INTO ActorMovieMapping
                                (ActorId, MovieId)
                                VALUES
                                ({actorId}, @insertedId)";
            }

            using(var connection = new SqlConnection(ConnectionString))
            {
                var insertedId = connection.QueryFirst<long>(insertMovieQuery, movie);
                connection.Execute(mapperQuery, new { insertedId });
                return insertedId;
            }
        }
    }
}
