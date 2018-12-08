using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Uni.Api.Web.Configurations
{
    public static class MvcOptionsExtensions
    {
        public static void UseCentralRoutePrefix([NotNull] this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            if (opts == null)
            {
                throw new ArgumentNullException(nameof(opts));
            }

            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }

        private sealed class RouteConvention : IApplicationModelConvention
        {
            private readonly AttributeRouteModel _centralPrefix;

            public RouteConvention([NotNull] IRouteTemplateProvider routeTemplateProvider)
            {
                if (routeTemplateProvider == null)
                {
                    throw new ArgumentNullException(nameof(routeTemplateProvider));
                }

                _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
            }

            public void Apply(ApplicationModel application)
            {
                var selectorModels = application.Controllers.SelectMany(x => x.Selectors);
                foreach (var selectorModel in selectorModels)
                {
                    if (selectorModel.AttributeRouteModel != null)
                    {
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
                            _centralPrefix,
                            selectorModel.AttributeRouteModel
                        );
                    }
                    else
                    {
                        selectorModel.AttributeRouteModel = _centralPrefix;
                    }
                }
            }
        }
    }
}
