namespace CineBaseV2.Model
{
    using System;

    public class MovieAggregate
    {
        public long Id { get; set; }

        public string MovieName { get; set; }

        public string Plot { get; set; }

        public string Image { get; set; }

        public string YearOfRelease { get; set; }

        public long ProducerId { get; set; }

        public long ActorId { get; set; }

        public string ActorBiography { get; set; }

        public DateTime ActorDateOfBirth { get; set; }

        public string ActorSex { get; set; }

        public string ActorName { get; set; }

        public string ProducerBiography { get; set; }

        public DateTime ProducerDateOfBirth { get; set; }

        public string ProducerSex { get; set; }

        public string ProducerName { get; set; }
    }
}
