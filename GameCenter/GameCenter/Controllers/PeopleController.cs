using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using GameCenter.Data;
using GameCenter.DTOs;
using GameCenter.Filters;
using GameCenter.Models;
using GameCenter.Services.Interface;
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
        private readonly IFileStorageService fileStorage;
        private readonly string containerName = "people";

        public PeopleController(AppDbContext context, ILogger<GenresController> logger, IMapper mapper, IFileStorageService fileStorage)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
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
                if(personCreationDto.Picture != null)
                {
                    using(var memoryStream = new MemoryStream())
                    {
                        personCreationDto.Picture.CopyTo(memoryStream);
                        var content = memoryStream.ToArray();
                        var extension = Path.GetExtension(personCreationDto.Picture.FileName);
                        person.Picture = await fileStorage.SaveFile(content, extension, containerName, personCreationDto.Picture.ContentType);
                    }
                }
                
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
        public async Task<ActionResult> Put(int id,[FromBody] PersonCreationDTO personCreationDto)
        {
            try
            {
                var personDb = await context.People.FirstOrDefaultAsync(x => x.Id == id);

                if(personDb == null) { return NotFound(); }

                personDb = mapper.Map(personCreationDto, personDb);

                if(personCreationDto.Picture != null)
                {
                    using(var memoryStream = new MemoryStream())
                    {
                        await personCreationDto.Picture.CopyToAsync(memoryStream);
                        var content = memoryStream.ToArray();
                        var extension = Path.GetExtension(personCreationDto.Picture.FileName);
                        personDb.Picture = await fileStorage.EditFile(content, extension, containerName, personDb.Picture, personCreationDto.Picture.ContentType);
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

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
                if(person == null) { return NotFound(); }

                context.Remove(person);
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