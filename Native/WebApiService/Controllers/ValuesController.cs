using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApiService.Infrastructure;
using WebApiService.Models;

namespace WebApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public ValuesController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<PointOfInterest> Get()
        {
            var results = _databaseContext.PointsOfInterest.ToList();
            return results;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost("Create")]
        public void Create(PointOfInterest value)
        {
            if (_databaseContext.PointsOfInterest.Any(x => x.Id == value.Id))
            {
                _databaseContext.Update(value);
            }
            else
            {
                _databaseContext.PointsOfInterest.Add(value);
            }

            _databaseContext.SaveChanges();
        }

        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            var poi = _databaseContext.PointsOfInterest.FirstOrDefault(x => x.Id == id);
            if (poi != null)
            {
                _databaseContext.PointsOfInterest.Remove(poi);
                _databaseContext.SaveChanges();
            }
        }
    }
}
