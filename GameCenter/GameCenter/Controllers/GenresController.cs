using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameCenter.Data;
using GameCenter.DTOs;
using GameCenter.Filters;
using GameCenter.Helpers;
using GameCenter.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : CustomBaseController
    {
        private readonly AppDbContext context;
        private readonly ILogger<GenresController> logger;
        private readonly IMapper mapper;

        public GenresController(AppDbContext context, ILogger<GenresController> logger, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet(Name = "getGenres")]
        [ServiceFilter(typeof(GenreHATEOASAttribute))]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            try
            {
                return await Get<Genre, GenreDTO>();

                /*if(includeHATEOS)
                {
                    var ResourceCollection = new ResourceCollection<GenreDTO>(genresDto);
                    genresDto.ForEach(genre => GenerateLinks(genre));
                    ResourceCollection.Links.Add(new Link(Url.Link("getGenres", new {}), rel: "self", method: "GET"));
                    ResourceCollection.Links.Add(new Link(Url.Link("createGenre", new {}), rel: "create-genre", method: "POST")); 
                    return Ok(ResourceCollection);               
                } */
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }
        }

        private void GenerateLinks(GenreDTO genreDto)
        {
            genreDto.Links.Add(new Link(Url.Link("getGenre", new { Id = genreDto.Id}), "get-genre", method: "GET"));
            genreDto.Links.Add(new Link(Url.Link("putGenre", new { Id = genreDto.Id}), "put-genre", method: "PUT"));
            genreDto.Links.Add(new Link(Url.Link("deleteGenre", new { Id = genreDto.Id}), "delete-genre", method: "DELETE"));
        }

        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GenreDTO), 200)]
        [HttpGet("{Id}", Name = "getGenre")]
        [ServiceFilter(typeof(GenreHATEOASAttribute))]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            try
            {
                return await Get<Genre, GenreDTO>(id);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }
        }

        [HttpPost(Name = "createGenre")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreationDto)
        {
            try
            {
                return await Post<GenreCreationDTO, Genre, GenreDTO>(genreCreationDto);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }   
        }

        [HttpPut("{Id}", Name = "putGenre")]
        public async Task<ActionResult> Put(int id,[FromBody] GenreCreationDTO genreCreationDto)
        {
            try
            {
                return await Put<GenreCreationDTO, Genre>(id, genreCreationDto);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }           
        }

        /// <summary>
        /// Delete a Genre
        /// </summary>
        /// <param name="id">Id of the genre to delete</param>
        /// <returns></returns>
        [HttpDelete("{Id}", Name = "deleteGenre")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                return await Delete<Genre>(id);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }
        }
    }
}