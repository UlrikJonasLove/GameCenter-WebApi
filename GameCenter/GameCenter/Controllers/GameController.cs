using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameCenter.Data;
using GameCenter.DTOs;
using GameCenter.Helpers;
using GameCenter.Models;
using GameCenter.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Logging;

namespace GameCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : CustomBaseController
    {
        private readonly ILogger logger;
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorage;
        private readonly string containerName = "Games";

        public GameController(ILogger logger, AppDbContext context, IMapper mapper, IFileStorageService fileStorage) : base(context, mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IndexGamePageDTO>> Get()
        {
            var top = 6;
            var today = DateTime.Today;
            var upcomingReleases = await context.Game
                .Where(x => x.ReleaseDate > today)
                .OrderBy(x => x.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var newlyReleases = await context.Game
                .Where(x => x.NewlyRelease)
                .Take(top)
                .ToListAsync();

            var result = new IndexGamePageDTO();
            result.NewlyReleases = mapper.Map<List<GameDTO>>(newlyReleases);
            result.UpcomingReleases = mapper.Map<List<GameDTO>>(upcomingReleases);
            return result;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<GameDTO>>> Filter([FromQuery] GameFilterDTO gameFilterDto)
        {
            try
            {

            }
            catch(Exception)
            {
                
            }
            var gamesQueryable = context.Game.AsQueryable();

            if(!string.IsNullOrWhiteSpace(gameFilterDto.Title))
            {
                gamesQueryable = gamesQueryable.Where(x => x.Title.Contains(gameFilterDto.Title));
            }

            if(gameFilterDto.NewlyReleases)
            {
                gamesQueryable = gamesQueryable.Where(x => x.NewlyRelease);
            }

            if(gameFilterDto.UpcomingReleases)
            {
                var today = DateTime.Today;
                gamesQueryable = gamesQueryable.Where(x => x.ReleaseDate > today);
            }

            if(gameFilterDto.GenreId != 0)
            {
                gamesQueryable = gamesQueryable
                    .Where(x => x.GamesGenres.Select(y => y.GenreId)
                    .Contains(gameFilterDto.GenreId));
            }

            if(!string.IsNullOrWhiteSpace(gameFilterDto.OrderingField))
            {
                try
                {
                    gamesQueryable = gamesQueryable
                        .OrderBy($"{gameFilterDto.OrderingField} {(gameFilterDto.AscendingOrder ? "ascending" : "descending")}");
                }
                catch(Exception)
                {
                   logger.LogWarning("Could not order by field: " + gameFilterDto.OrderingField);
                }           
            }

            await HttpContext.InsertPaginationParametersInResponse(gamesQueryable, gameFilterDto.ItemsPerPage);
            var games = await gamesQueryable.Paginate(gameFilterDto.pagination).ToListAsync();

            return mapper.Map<List<GameDTO>>(games);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<GameDetailDTO>> Get(int id)
        {
            var game = await context.Game
                .Include(x => x.GamesActors).ThenInclude(x => x.Person)
                .Include(x => x.GamesGenres).ThenInclude(x => x.Genre)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(game == null) { return NotFound(); }

            return mapper.Map<GameDetailDTO>(game);
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
                
                AnnotatePersonOrder(game);

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

        private static void AnnotatePersonOrder(Game game)
        {
            if(game.GamesActors != null)
            {
                for(int i = 0; i < game.GamesActors.Count; i++)
                {
                    game.GamesActors[i].Order = i;
                }
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

                await context.Database.ExecuteSqlInterpolatedAsync($"delete from GamesActors where GameId = {gameDb.Id}, delete from GamesGenres where GameId = {gameDb.Id}");
                AnnotatePersonOrder(gameDb);
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
            return await Patch<Game, GamePatchDTO>(id, patchDocument);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                return await Delete<Game>(id);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
            }
        }
    }
}