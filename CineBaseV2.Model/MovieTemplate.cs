namespace CineBaseV2.Model
{
    using System.Collections.Generic;

    public class MovieTemplate
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string YearOfRelease { get; set; }

        public string Plot { get; set; }

        public long ProducerId { get; set; }

        public List<long> ActorIdList { get; set; }
    }
}
