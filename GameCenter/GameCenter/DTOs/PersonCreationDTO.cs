using System;
using System.ComponentModel.DataAnnotations;
using GameCenter.Validations;
using GameCenter.Validator;
using Microsoft.AspNetCore.Http;

namespace GameCenter.DTOs
{
    public class PersonCreationDTO : PersonPatchDTO
    {
        [FileSizeValidator(4)]
        //[ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Picture { get; set; }
    }
}