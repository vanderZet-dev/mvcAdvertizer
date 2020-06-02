using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Config.TagHelpers
{
    public class SortHeaderTagHelper : TagHelper
    {
        public string CurrentSort { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {            
            if (CurrentSort.Equals("asc"))
            {
                output.Attributes.Add("class", "glyphicon-chevron-up glyphicon");
            }
            else output.Attributes.Add("class", "glyphicon-chevron-down glyphicon");
        }
    }
}
