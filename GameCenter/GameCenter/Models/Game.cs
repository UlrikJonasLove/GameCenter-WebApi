using System;
using System.ComponentModel.DataAnnotations;

namespace GameCenter.Models
{
    public class Game
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public string Summary { get; set; }        
        public bool NewlyRelease { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }       
    }
}