namespace CineBaseV2.DatabaseHandler.Interfaces
{
    using CineBaseV2.Model;
    using System.Collections.Generic;

    public interface IProducerDatabaseHandler
    {
        IEnumerable<Producer> GetProducers();

        Producer GetProducer(long id);

        long AddProducer(Producer actor);

        void DeleteProducer(long id);
    }
}
