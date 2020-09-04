using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcAdvertizer.Utils.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int maxFileSizeMB;
        private readonly string errorMessage;
        public MaxFileSizeAttribute(int maxFileSizeMB, string errorMessage) {
            this.maxFileSizeMB = maxFileSizeMB;
            this.errorMessage = errorMessage;
        }

        public MaxFileSizeAttribute(int maxFileSizeMB) {
            this.maxFileSizeMB = maxFileSizeMB;
            errorMessage = $"Maximum allowed file size is { maxFileSizeMB} bytes.";
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext) {
            var file = value as IFormFile;
            if (file != null)
            {
                var fileSizeMB = Convert.ToDouble(file.Length) / 1024.0 / 1024.0;
                if (fileSizeMB > maxFileSizeMB)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage() {
            return errorMessage;
        }
    }
}
