using System.Collections.Generic;
using GameCenter.Models;

namespace GameCenter.DTOs
{
    public class GameDetailDTO : GameDTO
    {
        public List<Genre> Genre { get; set; }
        public List<ActorDTO> Actor { get; set; }
    }
}