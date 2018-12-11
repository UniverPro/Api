using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Uni.Identity.Web.MVC.TagHelpers
{
    /// <summary>
    ///     Достаёт значение Name из атрибута <see cref="T:System.ComponentModel.DataAnnotations.DisplayAttribute" /> если он
    ///     указан для свойства модели. В противном случае использует название указанного свойства модели. Дополняет содержимое
    ///     контента html-элемента извлечённым значением.
    /// </summary>
    /// <inheritdoc />
    [PublicAPI]
    [HtmlTargetElement(Attributes = NameForAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class NameForTagHelper : TagHelper
    {
        private const string NameForAttributeName = "cd-name-for";

        /// <summary>
        ///     Достаёт значение Name из атрибута <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute" /> если он
        ///     указан для свойства модели. В противном случае использует название указанного свойства модели. Дополняет содержимое
        ///     контента html-элемента извлечённым значением.
        /// </summary>
        [HtmlAttributeName(NameForAttributeName)]
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
                throw new InvalidOperationException($"No metadata for ({NameForAttributeName})");
            }

            var name = metadata.DisplayName;
            if (string.IsNullOrEmpty(name))
            {
                name = metadata.PropertyName;
            }

            output.Content.Append(name);
        }
    }
}
