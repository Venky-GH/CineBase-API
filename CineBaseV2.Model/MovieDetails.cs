namespace CineBaseV2.Model
{
    using System.Collections.Generic;

    public class MovieDetails
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string YearOfRelease { get; set; }

        public string Plot { get; set; }

        public string Image { get; set; }

        public List<Actor> Actors { get; set; }

        public Producer Producer { get; set; }
    }
}
