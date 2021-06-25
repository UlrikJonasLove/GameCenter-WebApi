using System;
using System.ComponentModel.DataAnnotations;

namespace GameCenter.DTOs
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}