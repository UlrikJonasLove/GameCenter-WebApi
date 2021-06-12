using System.Collections.Generic;

namespace GameCenter.DTOs
{
    public class IndexGamePageDTO
    {
        public List<GameDTO> UpcomingReleases { get; set; }
        public List<GameDTO> NewlyReleases { get; set; }
    }
}