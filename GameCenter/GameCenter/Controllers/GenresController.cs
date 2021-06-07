using System.Collections.Generic;
using System.Threading.Tasks;
using GameCenter.Models;
using GameCenter.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IRepository repository;

        public GenresController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            return await repository.GetAllGenres();
        }

        [HttpGet("{Id}")]
        public ActionResult<Genre> Get(int id)
        {
            var genre = repository.GetGenreById(id);
            if(genre == null) 
            {
                return NotFound();
            }
            return genre;
        }

        [HttpPost]
        public ActionResult Post()
        {
            return NoContent();
        }

        [HttpPut]
        public ActionResult Put()
        {
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return NoContent();
        }
    }
}