using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.AdditionalObjects
{
    public class RecaptchaResponse
    {
        public string Response { get; set; }

        public string ConnectionRemoteIpAddress { get; set; }

        public bool Valid { get; set; }

        public bool ValidateModelState(ModelStateDictionary modelsTate) {

            if (!Valid)
            {
                modelsTate.AddModelError("Recaptcha", "Check request return false value.");
            }

            return Valid;
        }
    }
}
