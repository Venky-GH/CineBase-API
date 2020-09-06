using CineBaseV2.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineBaseV2.DatabaseHandler.Interfaces
{
    public interface IMovieDatabaseHandler
    {
        IEnumerable<MovieAggregate> GetMovieAggregates(long id = -1);

        void UpdateMovie(Movie movie, List<long> actorIds);

        long AddMovie(Movie movie, List<long> actorIds);
    }
}
