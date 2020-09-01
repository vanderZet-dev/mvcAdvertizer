using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Data.Models;
using System.Collections.Generic;

namespace MvcAdvertizer.Config.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PaginatedList<Advert> PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";
                        
            TagBuilder tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");

            var totalPages = PageModel.TotalPages;
                        
            TagBuilder currentItem = CreateTag(PageModel.PageIndex, urlHelper);

            if(totalPages <= 10)
            {
                for (int i = 1; i <= totalPages; i++)
                {
                    if (i == PageModel.PageIndex)
                    {
                        tag.InnerHtml.AppendHtml(currentItem);
                    }
                    else
                    {
                        TagBuilder newItem = CreateTag(i, urlHelper);
                        tag.InnerHtml.AppendHtml(newItem);
                    }
                }
            }
            else
            {
                int maxCounter = 3;
                int currentCounter = 0;                
                if (PageModel.PageIndex >=5)
                {
                    TagBuilder newItem = CreateTag(1, urlHelper);
                    tag.InnerHtml.AppendHtml(newItem);
                    if (PageModel.PageIndex > 5)
                    {                        
                        tag.InnerHtml.AppendHtml(CreateTag(2, urlHelper));
                    }
                    if (PageModel.PageIndex > 6)
                    {
                        tag.InnerHtml.AppendHtml("<span style=\"margin-left:5px; margin-right:5px;\">...</span>");
                    }                    
                }

                //---------------------------------------------------------------------------

                var newTags = new List<TagBuilder>();
                for (int i = PageModel.PageIndex - 1; i >= 1; i--)
                {
                    if (++currentCounter <= maxCounter)
                    {       
                        newTags.Add(CreateTag(i, urlHelper));
                    }
                    else break;
                }
                newTags.Reverse();
                foreach(var newTag in newTags)
                {
                    tag.InnerHtml.AppendHtml(newTag);
                }

                //---------------------------------------------------------------------------
                if ((totalPages - PageModel.PageIndex) < 6)
                {
                    for (int i = PageModel.PageIndex; i <= totalPages; i++)
                    {                        
                        if (i == PageModel.PageIndex)
                        {
                            tag.InnerHtml.AppendHtml(currentItem);
                        }
                        else
                        {
                            TagBuilder newItem = CreateTag(i, urlHelper);
                            tag.InnerHtml.AppendHtml(newItem);
                        }                        
                    }
                }
                else
                {
                    maxCounter = 4;
                    currentCounter = 0;
                    for (int i = PageModel.PageIndex; i <= totalPages; i++)
                    {
                        if (++currentCounter <= maxCounter)
                        {
                            if (i == PageModel.PageIndex)
                            {
                                tag.InnerHtml.AppendHtml(currentItem);
                            }
                            else
                            {
                                TagBuilder newItem = CreateTag(i, urlHelper);
                                tag.InnerHtml.AppendHtml(newItem);
                            }
                        }
                        else break;
                    }
                    tag.InnerHtml.AppendHtml("<span style=\"margin-left:5px; margin-right:5px;\">...</span>");
                    tag.InnerHtml.AppendHtml(CreateTag(totalPages - 1, urlHelper));
                    tag.InnerHtml.AppendHtml(CreateTag(totalPages, urlHelper));
                }
                
            }           
                       
            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNum, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");
            if (pageNum == this.PageModel.PageIndex)
            {
                item.AddCssClass("active");
            }
            else
            {
                PageUrlValues["pageNumber"] = pageNum;                
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }
            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append(pageNum.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
