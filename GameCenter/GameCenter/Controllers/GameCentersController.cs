using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameCenter.Data;
using GameCenter.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GameCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameCentersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;

        public GameCentersController(IMapper mapper, AppDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameCenterDTO>>> Get([FromQuery] GameCenterFilterDTO gameCenterFilterDto)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var usersLocation = geometryFactory.CreatePoint(new Coordinate(gameCenterFilterDto.Long, gameCenterFilterDto.Lat));

            var centers = await context.GameCenters
                .OrderBy(x => x.Location.Distance(usersLocation))
                .Where(x => x.Location.IsWithinDistance(usersLocation, gameCenterFilterDto.DistanceInKms * 1000))
                .Select(x => new GameCenterDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    DistanceInMeters = Math.Round(x.Location.Distance(usersLocation))
                }).ToListAsync();

            return centers;
        }
    }
}