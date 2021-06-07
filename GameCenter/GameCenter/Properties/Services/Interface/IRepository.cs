using System.Collections.Generic;
using System.Threading.Tasks;
using GameCenter.Models;

namespace GameCenter.Services.Interface
{
    public interface IRepository
    {
        Task<List<Genre>> GetAllGenres();
        Genre GetGenreById(int Id);
    }
}