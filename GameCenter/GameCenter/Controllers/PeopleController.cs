using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameCenter.Data;
using GameCenter.DTOs;
using GameCenter.Filters;
using GameCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger<GenresController> logger;
        private readonly IMapper mapper;

        public PeopleController(AppDbContext context, ILogger<GenresController> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonDTO>>> Get()
        {
            try
            {
                var people = await context.People.AsNoTracking().ToListAsync();
                var peopleDto = mapper.Map<List<PersonDTO>>(people);
                return peopleDto; 
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<PersonDTO>> Get(int id)
        {
            try
            {
                var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
                if(person == null) { return NotFound(); }

                var personDto = mapper.Map<PersonDTO>(person);
                return personDto;
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PersonCreationDTO personCreationDto)
        {
            try
            {
                var person = mapper.Map<Person>(personCreationDto);
                context.Add(person);
                await context.SaveChangesAsync();
                var personDto = mapper.Map<PersonDTO>(person);
                return new CreatedAtRouteResult(new {personDto.Id}, personDto);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }   
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Put(int id,[FromBody] GenreCreationDTO genreCreationDto)
        {
            try
            {
                var genre = mapper.Map<Genre>(genreCreationDto);
                genre.Id = id;
                context.Entry(genre).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }           
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == id);
                if(genre == null) { return NotFound(); }

                context.Remove(genre);
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }
        }
    }
}