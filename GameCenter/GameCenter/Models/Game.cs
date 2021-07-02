using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameCenter.Models
{
    public class Game : IID
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public string Summary { get; set; }        
        public bool NewlyRelease { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }       
        public List<GamesActors> GamesActors { get; set;}
        public List<GamesGenres> GamesGenres { get; set;}
    }
}