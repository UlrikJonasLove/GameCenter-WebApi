using System;

namespace GameCenter.DTOs
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }        
        public bool NewlyRelease { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
    }
}