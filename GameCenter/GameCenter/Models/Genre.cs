using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameCenter.Validations;

namespace GameCenter.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        [FirstletterUppercase]
        public string Name { get; set; }
        public List<GamesGenres> GamesGenres { get; set; }
    }
}