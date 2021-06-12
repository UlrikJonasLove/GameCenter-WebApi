using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParametersInResponse<T>(this HttpContext httpContext, IQueryable<T> queryable, int itemsPerPage)
        {
            if(httpContext == null) { throw new ArgumentException(nameof(httpContext)); };

            double count = await queryable.CountAsync();
            double totalAmountOfPages = Math.Ceiling(count / itemsPerPage);
            httpContext.Response.Headers.Add("totalAmountOfPages", totalAmountOfPages.ToString());
        }
    }
}