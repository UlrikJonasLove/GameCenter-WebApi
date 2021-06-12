using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GameCenter.Validations
{
    public class FileSizeValidator : ValidationAttribute
    {
        private readonly int maxFileSizeInMbs;

        public FileSizeValidator(int MaxFileSizeInMbs)
        {
            maxFileSizeInMbs = MaxFileSizeInMbs;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if(formFile == null)
            {
                return ValidationResult.Success;
            }

            if(formFile.Length > maxFileSizeInMbs * 1024 * 1024)
            {
                return new ValidationResult($"File Size cannot be bigger than {maxFileSizeInMbs} megabytes");
            }

            return ValidationResult.Success;
        }
    }
}