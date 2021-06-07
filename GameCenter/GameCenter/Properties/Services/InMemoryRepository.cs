using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCenter.Models;
using GameCenter.Services.Interface;

namespace GameCenter.Services
{
    public class InMemoryRepository : IRepository
    {
        private List<Genre> genres;
        public InMemoryRepository()
        {
            genres = new List<Genre>()
            {
                new Genre() {Id = 1, Name = "Fantasy"},
                new Genre() {Id = 2, Name = "RPG"}
            };
        }
        public async Task<List<Genre>> GetAllGenres()
        {
            await Task.Delay(3000);
            return genres;
        }

        public Genre GetGenreById(int Id)
        {
            return genres.FirstOrDefault(x => x.Id == Id);
        }
    }
}