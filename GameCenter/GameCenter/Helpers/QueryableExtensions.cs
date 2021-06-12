using System.Linq;
using GameCenter.DTOs;

namespace GameCenter.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.ItemsPerPage)
                .Take(paginationDTO.ItemsPerPage);
        }
    }
}