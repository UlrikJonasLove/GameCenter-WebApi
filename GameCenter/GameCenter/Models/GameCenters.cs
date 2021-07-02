using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace GameCenter.Models
{
    public class GameCenters
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Point Location { get; set; }
    }
}