using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using StoreApp.Web.Models;

namespace StoreApp.Web.TagHelpers;

[HtmlTargetElement("div", Attributes = "page-model")]
public class PageLinkTagHelper : TagHelper
{
    private IUrlHelperFactory _urlHelperFactory;

    public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }

    [ViewContext]
    public ViewContext? ViewContext { get; set; }
    public PageInfo? PageModel { get; set; }
    public string? PageAction { get; set; }
    public string PageClass { get; set; } = string.Empty;
    public string PageClassLink { get; set; } = string.Empty;
    public string PageClassActive { get; set; } = string.Empty;
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext != null && PageModel != null)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder tag = new("div");

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder item = new("a");
                item.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
                item.AddCssClass(PageClass);
                item.AddCssClass(i == PageModel.CurrentPage ? PageClassActive : PageClassLink); // if the page is current page, set it's css to active, if not set it to link
                item.InnerHtml.Append(i.ToString());
                tag.InnerHtml.AppendHtml(item);
            }
            
            output.Content.AppendHtml(tag.InnerHtml);
        }
    }
}