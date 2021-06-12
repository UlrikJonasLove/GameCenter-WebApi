using System;
using System.ComponentModel.DataAnnotations;
using GameCenter.Validations;
using GameCenter.Validator;
using Microsoft.AspNetCore.Http;

namespace GameCenter.DTOs
{
    public class PersonCreationDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        [FileSizeValidator(4)]
        //[ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Picture { get; set; }      
    }
}