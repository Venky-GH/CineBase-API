namespace CineBaseV2.Controllers
{
    using System;
    using System.Linq;
    using CineBaseV2.DatabaseHandler.Interfaces;
    using CineBaseV2.Model;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorDatabaseHandler _actorDatabaseHandler;

        public ActorsController(IActorDatabaseHandler actorDatabaseHandler)
        {
            _actorDatabaseHandler = actorDatabaseHandler;
        }

        // GET: api/Actors
        [HttpGet]
        public IActionResult GetActors()
        {
            var actors = _actorDatabaseHandler.GetActors().ToList();
            return Ok(new
            {
                message = actors.Count + " actors fetched successfully",
                result = actors
            });
        }

        // GET: api/Actors/5
        [HttpGet("{id}")]
        public ActionResult<Actor> GetActor(long id)
        {
            var actor = _actorDatabaseHandler.GetActor(id);

            if (actor == null)
            {
                return NotFound(new
                {
                    message = "Actor with id " + id + " not found"
                });
            }

            return Ok(new
            {
                message = "Actor with id " + id + " found",
                result = actor
            });
        }

        // POST: api/Actors
        [HttpPost]
        public IActionResult PostActor(Actor actor)
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
                var response = _actorDatabaseHandler.AddActor(actor);

                return Ok(new
                {
                    message = "Actor added successfully",
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

        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        public IActionResult DeleteActor(long id)
        {
            try
            {
                _actorDatabaseHandler.DeleteActor(id);

                return Ok(new
                {
                    message = "Actor with id " + id + " deleted successfully"
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, new
                {
                    message = "Something went wrong"
                });
            }
        }
    }
}
