using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Uni.Identity.Web.MVC.TagHelpers
{
    /// <summary>
    ///     Достаёт значение Name из атрибута <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute" /> если он
    ///     указан для свойства модели. В противном случае использует название указанного свойства модели. Создаёт html-атрибут
    ///     placeholder и заполняет его извлечённым значением.
    /// </summary>
    /// <inheritdoc />
    [PublicAPI]
    [HtmlTargetElement("input", Attributes = PlaceholderForAttributeName)]
    public class PlaceholderForTagHelper : TagHelper
    {
        private const string PlaceholderForAttributeName = "cd-placeholder-for";

        /// <summary>
        ///     Достаёт значение Name из атрибута <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute" /> если он
        ///     указан для свойства модели. В противном случае использует название указанного свойства модели. Создаёт html-атрибут
        ///     placeholder и заполняет его извлечённым значением.
        /// </summary>
        [HtmlAttributeName(PlaceholderForAttributeName)]
        public ModelExpression PlaceholderFor { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var metadata = PlaceholderFor.Metadata;
            if (metadata == null)
            {
                throw new InvalidOperationException($"No metadata for ({PlaceholderForAttributeName})");
            }

            var name = metadata.DisplayName;
            if (string.IsNullOrEmpty(name))
            {
                name = metadata.PropertyName;
            }

            output.Attributes.SetAttribute("placeholder", name);
        }
    }
}
