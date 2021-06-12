using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameCenter.Helpers;
using GameCenter.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.DTOs
{
    public class GameCreationDTO : GamePatchDTO
    {
        [FileSizeValidator(4)]
        //[ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Poster { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenresIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorDTO>>))]
        public List<ActorDTO> Actors { get; set; }     
    }
}