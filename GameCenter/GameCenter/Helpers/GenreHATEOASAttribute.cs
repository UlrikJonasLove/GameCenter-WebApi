using System.Threading.Tasks;
using GameCenter.DTOs;
using GameCenter.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameCenter.Helpers
{
    public class GenreHATEOASAttribute : HATEOASAttribute
    {
        private readonly LinksGenerator linksGenerator;

        public GenreHATEOASAttribute(LinksGenerator linksGenerator)
        {
            this.linksGenerator = linksGenerator;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var includeHATEOAS = ShouldIncludeHATEOS(context);

            if(!includeHATEOAS)
            {
                await next();
                return;
            }

            await linksGenerator.Generate<GenreDTO>(context, next);
        }
    }
}