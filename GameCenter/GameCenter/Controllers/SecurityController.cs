using System;
using System.Threading.Tasks;
using GameCenter.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly IDataProtector _protector;
        private readonly HashService hashService;

        public SecurityController(IDataProtectionProvider protectionProvider, HashService hashService)
        {
            _protector = protectionProvider.CreateProtector("value_secret_and_unique");
            this.hashService = hashService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string plainText = "Ulrik";
            string encryptedText = _protector.Protect(plainText);
            string decryptedText = _protector.Unprotect(encryptedText);

            return Ok(new { plainText, encryptedText, decryptedText });
        }

        [HttpGet("TimeBound")]
        public async Task<IActionResult> GetTimeBound()
        {
            var protectorTimeBound = _protector.ToTimeLimitedDataProtector();
            string plainText = "Ulrik";
            string encryptedText = protectorTimeBound.Protect(
                plainText, lifetime: TimeSpan.FromSeconds(5));
            await Task.Delay(6000);
            string decryptedText = protectorTimeBound.Unprotect(encryptedText);

            return Ok(new { plainText, encryptedText, decryptedText });
        }

        [HttpGet("hash")]
        public IActionResult GetHash()
        {
            var plainText = "Ulrik";
            var hashResult1 = hashService.Hash(plainText);
            var hashResult2 = hashService.Hash(plainText);

            return Ok( new { hashResult1, hashResult2 });
        }
    }
}