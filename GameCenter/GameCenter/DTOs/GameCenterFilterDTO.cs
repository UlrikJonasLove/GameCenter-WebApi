using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GameCenter.DTOs
{
    public class GameCenterFilterDTO
    {
        [BindRequired]
        [Range(-180, 180)]
        public double Lat { get; set; }
        [BindRequired]
        [Range(-180, 180)]
        public double Long { get; set; }
        public int distanceInKms = 10;
        public int maxDistanceInKms = 50;
        public int DistanceInKms 
        { 
            get 
            { 
                return distanceInKms; 
            } 
            set
            {
                distanceInKms = (value > maxDistanceInKms) ? maxDistanceInKms : value;
            }
        }   
    }
}