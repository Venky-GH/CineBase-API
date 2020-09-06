namespace CineBaseV2.DatabaseHandler.Interfaces
{
    using CineBaseV2.Model;
    using System.Collections.Generic;

    public interface IActorDatabaseHandler
    {
        IEnumerable<Actor> GetActors();

        Actor GetActor(long id);

        long AddActor(Actor actor);

        void DeleteActor(long id);
    }
}
