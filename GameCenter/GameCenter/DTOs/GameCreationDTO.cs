using System;
using System.ComponentModel.DataAnnotations;
using GameCenter.Validations;
using Microsoft.AspNetCore.Http;

namespace GameCenter.DTOs
{
    public class GameCreationDTO : GamePatchDTO
    {
        [FileSizeValidator(4)]
        //[ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Poster { get; set; }
    }
}