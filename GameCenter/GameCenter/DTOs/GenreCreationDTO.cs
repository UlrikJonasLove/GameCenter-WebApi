using System.ComponentModel.DataAnnotations;
using GameCenter.Validations;

namespace GameCenter.DTOs
{
    public class GenreCreationDTO
    {
        [Required]
        [StringLength(40)]
        [FirstletterUppercase]
        public string Name { get; set; }
    }
}