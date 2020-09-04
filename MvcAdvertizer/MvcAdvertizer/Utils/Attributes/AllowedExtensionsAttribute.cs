using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace MvcAdvertizer.Utils.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] extensions;
        private readonly string errorMessage;
        public AllowedExtensionsAttribute(string[] extensions, string errorMessage) {
            this.extensions = extensions;
            this.errorMessage = errorMessage;
        }

        public AllowedExtensionsAttribute(string[] extensions) {
            this.extensions = extensions;
            errorMessage = $"This photo extension is not allowed!";
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext) {
            var file = value as IFormFile;            
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!extensions.Contains(extension.ToLower()))
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
