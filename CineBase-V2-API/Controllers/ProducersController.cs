using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CineBaseV2.DatabaseHandler;
using CineBaseV2.DatabaseHandler.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CineBaseV2.Model;
using System.Linq;

namespace CineBaseV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducersController : ControllerBase
    {
        private readonly IProducerDatabaseHandler _producerDatabaseHandler;

        public ProducersController(IProducerDatabaseHandler producerDatabaseHandler)
        {
            _producerDatabaseHandler = producerDatabaseHandler;
        }

        // GET: api/Actors
        [HttpGet]
        public IActionResult GetProducers()
        {
            var producers = _producerDatabaseHandler.GetProducers().ToList();
            return Ok(new
            {
                message = producers.Count + " producers fetched successfully",
                result = producers
            });
        }

        // GET: api/Actors/5
        [HttpGet("{id}")]
        public ActionResult<Producer> GetProducer(long id)
        {
            var producer = _producerDatabaseHandler.GetProducer(id);

            if (producer == null)
            {
                return NotFound(new
                {
                    message = "Producer with id " + id + " not found"
                });
            }

            return Ok(new
            {
                message = "Producer with id " + id + " found",
                result = producer
            });
        }

        // POST: api/Actors
        [HttpPost]
        public IActionResult PostProducer(Producer producer)
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
                var response = _producerDatabaseHandler.AddProducer(producer);

                return Ok(new
                {
                    message = "Producer added successfully",
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
        public IActionResult DeleteProducer(long id)
        {
            try
            {
                _producerDatabaseHandler.DeleteProducer(id);

                return Ok(new
                {
                    message = "Producer with id " + id + " deleted successfully"
                });
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                return StatusCode(500, new
                {
                    message = "Something went wrong"
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
