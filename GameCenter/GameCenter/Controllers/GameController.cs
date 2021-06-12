using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using GameCenter.Data;
using GameCenter.DTOs;
using GameCenter.Models;
using GameCenter.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorage;
        private readonly string containerName = "Games";

        public GameController(AppDbContext context, IMapper mapper, IFileStorageService fileStorage)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameDTO>>> Get()
        {
            var games = await context.Game.ToListAsync();
            return mapper.Map<List<GameDTO>>(games);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<GameDTO>> Get(int id)
        {
            var game = await context.Game.FirstOrDefaultAsync(x => x.Id == id);
            if(game == null) { return NotFound(); }

            return mapper.Map<GameDTO>(game);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] GameCreationDTO gameCreationDto)
        {
            try
            {
                var game = mapper.Map<Game>(gameCreationDto);
                if(gameCreationDto.Poster != null)
                {
                    using(var memoryStream = new MemoryStream())
                    {
                        gameCreationDto.Poster.CopyTo(memoryStream);
                        var content = memoryStream.ToArray();
                        var extension = Path.GetExtension(gameCreationDto.Poster.FileName);
                        game.Poster = await fileStorage.SaveFile(content, extension, containerName, gameCreationDto.Poster.ContentType);
                    }
                }
                
                context.Add(game);
                await context.SaveChangesAsync();
                var gameDto = mapper.Map<GameDTO>(game);
                return new CreatedAtRouteResult(new {gameDto.Id}, gameDto);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }   
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Put(int id,[FromForm] GameCreationDTO gameCreationDto)
        {
            try
            {
                var gameDb = await context.Game.FirstOrDefaultAsync(x => x.Id == id);
                if(gameDb == null) { return NotFound(); };

                gameDb = mapper.Map(gameCreationDto, gameDb);

                if(gameCreationDto.Poster != null)
                {
                    using(var memoryStream = new MemoryStream())
                    {
                        await gameCreationDto.Poster.CopyToAsync(memoryStream);
                        var content = memoryStream.ToArray();
                        var extension = Path.GetExtension(gameCreationDto.Poster.FileName);
                        gameDb.Poster = await fileStorage.SaveFile(content, extension, containerName, gameCreationDto.Poster.ContentType);
                    }
                }

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }           
        }

        [HttpPatch("{Id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<GamePatchDTO> patchDocument)
        {
            if(patchDocument == null) { return BadRequest(); }

            var entityFromDb = await context.Game.FirstOrDefaultAsync(x => x.Id == id);
            if(entityFromDb == null) { return NotFound(); }

            var entityDto = mapper.Map<GamePatchDTO>(entityFromDb);

            patchDocument.ApplyTo(entityDto, ModelState);

            var isValid = TryValidateModel(entityDto);

            if(!isValid) { return BadRequest(ModelState); }

            mapper.Map(entityDto, entityFromDb);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var game = await context.Game.FirstOrDefaultAsync(x => x.Id == id);
                if(game == null) { return NotFound(); }

                context.Remove(game);
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