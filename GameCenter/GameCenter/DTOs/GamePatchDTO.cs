using System;
using System.ComponentModel.DataAnnotations;
using GameCenter.Validations;
using Microsoft.AspNetCore.Http;

namespace GameCenter.DTOs
{
    public class GamePatchDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public string Summary { get; set; }        
        public bool NewlyRelease { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}