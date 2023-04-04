using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreTestApp.TagHelpers
{
    /// <summary>
    /// TagHelpers are used for smaller reusable components, 
    /// as opposed to ViewComponents which can contain more functionality
    /// </summary>
    public class EmailTagHelper : TagHelper
    {
        public string? Address { get; set; }
        public string? Content { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            output.Attributes.SetAttribute("href", "mailto:" + Address);
            output.Content.SetContent(Content);
        }
    }
}
