using System.Linq;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Uni.Api.Web.Configurations.Filters
{
    [UsedImplicitly]
    internal sealed class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var swaggerVersion = swaggerDoc.Info.Version;
            const string versionPlainText = "v{version}";

            swaggerDoc.Paths = swaggerDoc.Paths.ToDictionary(
                path => path.Key.Replace(versionPlainText, swaggerVersion),
                path => path.Value
            );
        }
    }
}
