using Microsoft.AspNetCore.Razor.TagHelpers;

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
