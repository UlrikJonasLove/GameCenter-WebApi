using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameCenter.Data;
using GameCenter.DTOs;
using GameCenter.Helpers;
using GameCenter.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public CustomBaseController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>() where TEntity : class
        {
            var models = await context.Set<TEntity>().AsNoTracking().ToListAsync();
            var dtos = mapper.Map<List<TDTO>>(models);
            return dtos;
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>(PaginationDTO paginationDto) where TEntity : class
        {
           var queryable = context.Set<TEntity>().AsNoTracking().AsQueryable();
           await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDto.ItemsPerPage);
           var models = await queryable.Paginate(paginationDto).ToListAsync();
           return mapper.Map<List<TDTO>>(models);
        }

        protected async Task<ActionResult<TDTO>> Get<TEntity, TDTO>(int id) where TEntity : class, IID
        {

            var model = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if(model == null) { return NotFound(); }

            return mapper.Map<TDTO>(model);
        }

        protected async Task<ActionResult> Post<TCreation, TEntity, TRead>(TCreation creation) where TEntity : class, IID
        {
            var model = mapper.Map<Genre>(creation);
            context.Add(model);
            await context.SaveChangesAsync();
            var genreDto = mapper.Map<GenreDTO>(model);
            return new CreatedAtRouteResult(new {model.Id}, genreDto);
        }

        protected async Task<ActionResult> Put<TCreation, TEntity>(int id, TCreation creation) where TEntity : class, IID
        {
                var model = mapper.Map<TEntity>(creation);
                model.Id = id;
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();       
        }

        protected async Task<ActionResult> Delete<TEntity>(int id) where TEntity : class, IID, new()
        {
                var model = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
                if(model == null) { return NotFound(); }

                context.Remove(model);
                await context.SaveChangesAsync();
                return NoContent();
        }

        protected async Task<ActionResult> Patch<TEntity, TDTO>(int id, JsonPatchDocument<TDTO> patchDocument)  where TDTO : class 
            where TEntity : class, IID
        {
            if(patchDocument == null) { return BadRequest(); }

            var entityFromDb = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if(entityFromDb == null) { return NotFound(); }

            var entityDto = mapper.Map<TDTO>(entityFromDb);

            patchDocument.ApplyTo(entityDto, ModelState);

            var isValid = TryValidateModel(entityDto);

            if(!isValid) { return BadRequest(ModelState); }

            mapper.Map(entityDto, entityFromDb);

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
