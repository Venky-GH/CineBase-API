namespace CineBaseV2.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CineBaseV2.DatabaseHandler.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using CineBaseV2.Model;

    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieDatabaseHandler _movieDatabaseHandler;

        public MoviesController(IMovieDatabaseHandler movieDatabaseHandler)
        {
            _movieDatabaseHandler = movieDatabaseHandler;
        }

        // GET: api/Movies
        [HttpGet]
        public JsonResult GetMovies()
        {
            var movieAggregates = _movieDatabaseHandler.GetMovieAggregates();
            var movieDetailsList = new List<MovieDetails>();
            var movieGroups = movieAggregates.GroupBy(aggr => aggr.Id).ToList();

            foreach (var movieGroup in movieGroups)
            {
                var groups = movieGroup.ToList();
                var currentMovie = groups[0];
                var movieDetails = new MovieDetails
                {
                    Name = currentMovie.MovieName,
                    Plot = currentMovie.Plot,
                    Image = currentMovie.Image,
                    YearOfRelease = currentMovie.YearOfRelease,
                    Producer = new Producer
                    {
                        Id = currentMovie.ProducerId,
                        Biography = currentMovie.ProducerBiography,
                        Name = currentMovie.ProducerName,
                        DateOfBirth = currentMovie.ProducerDateOfBirth,
                        Sex = currentMovie.ProducerSex
                    },
                    Actors = new List<Actor>()
                };

                var actorList = new List<Actor>();

                foreach(var group in groups)
                {
                    movieDetails.Actors.Add(new Actor
                    {
                        Id = group.ActorId,
                        Name = group.ActorName,
                        Sex = group.ActorSex,
                        Biography = group.ActorBiography,
                        DateOfBirth = group.ActorDateOfBirth
                    });
                }

                movieDetailsList.Add(movieDetails);
            }
            return new JsonResult(new
            {
                message = movieDetailsList.Count + " movies fetched successfully",
                result = movieDetailsList
            });
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public IActionResult GetMovie(long id)
        {
            var movieAggregates = _movieDatabaseHandler.GetMovieAggregates(id).ToList();

            if (movieAggregates.Count <= 0)
            {
                return NotFound(new
                {
                    message = "Movie with id " + id + " not found"
                });
            }

            var movie = movieAggregates[0];

            var movieDetails = new MovieDetails
            {
                Id = movie.Id,
                Image = movie.Image,
                Name = movie.MovieName,
                Plot = movie.Plot,
                YearOfRelease = movie.YearOfRelease,
                Producer = new Producer
                {
                    Id = movie.ProducerId,
                    Name = movie.ProducerName,
                    Biography = movie.ProducerBiography,
                    DateOfBirth = movie.ProducerDateOfBirth,
                    Sex = movie.ProducerSex
                },
                Actors = new List<Actor>()
            };

            foreach(var item in movieAggregates)
            {
                movieDetails.Actors.Add(new Actor
                {
                    Id = item.ActorId,
                    Name = item.ActorName,
                    Sex = item.ActorSex,
                    Biography = item.ActorBiography,
                    DateOfBirth = item.ActorDateOfBirth
                });
            }

            return Ok(new
            {
                message = "Movie with id " + id + " found",
                result = movieDetails
            });
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public IActionResult PutMovie(long id, MovieTemplate movieTemplate)
        {
            if (!ModelState.IsValid || id != movieTemplate.Id)
            {
                return BadRequest(new
                {
                    message = "Invalid input"
                });
            }

            var movie = new Movie
            {
                Id = movieTemplate.Id,
                Name = movieTemplate.Name,
                Image = movieTemplate.Image,
                YearOfRelease = movieTemplate.YearOfRelease,
                Plot = movieTemplate.Plot,
                ProducerId = movieTemplate.ProducerId
            };

            var actorIdList = movieTemplate.ActorIdList;

            try
            {
                _movieDatabaseHandler.UpdateMovie(movie, actorIdList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return StatusCode(500, new
                {
                    message = "Something went wrong"
                });
            }

            return Ok(new
            {
                message = "Movie updated successfully"
            });
        }

        // POST: api/Movies
        [HttpPost]
        public IActionResult PostMovie(MovieTemplate movieTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid input, please provide all the field values"
                });
            }

            try
            {
                var movie = new Movie
                {
                    Id = movieTemplate.Id,
                    Name = movieTemplate.Name,
                    Image = movieTemplate.Image,
                    YearOfRelease = movieTemplate.YearOfRelease,
                    Plot = movieTemplate.Plot,
                    ProducerId = movieTemplate.ProducerId
                };

                var actorIdList = movieTemplate.ActorIdList;

                var response = _movieDatabaseHandler.AddMovie(movie, actorIdList);

                return Ok(new
                {
                    message = "Movie added successfully",
                    id = response
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return StatusCode(500, new
                {
                    message = "Something went wrong"
                });
            }
        }
    }
}
