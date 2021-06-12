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
    public class GenresController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger<GenresController> logger;
        private readonly IMapper mapper;

        public GenresController(AppDbContext context, ILogger<GenresController> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            try
            {
                var genres = await context.Genres.AsNoTracking().ToListAsync();
                var genresDto = mapper.Map<List<GenreDTO>>(genres);
                return genresDto; 
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            try
            {
                var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == id);
                if(genre == null) 
                {
                    return NotFound();
                }
                var genreDto = mapper.Map<GenreDTO>(genre);
                return genreDto;
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreationDto)
        {
            try
            {
                var genre = mapper.Map<Genre>(genreCreationDto);
                context.Add(genre);
                await context.SaveChangesAsync();
                var genreDto = mapper.Map<GenreDTO>(genre);
                return new CreatedAtRouteResult(new {genreDto.Id}, genreDto);
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