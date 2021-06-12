using System;
using System.ComponentModel.DataAnnotations;
using GameCenter.Validations;
using GameCenter.Validator;
using Microsoft.AspNetCore.Http;

namespace GameCenter.DTOs
{
    public class PersonPatchDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}