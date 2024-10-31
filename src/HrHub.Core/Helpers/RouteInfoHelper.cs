using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HrHub.Core.Helpers
{
    public static class RouteInfoHelper
    {
        private static IActionDescriptorCollectionProvider actionDescriptorCollectionProvider;
        public static void Configure(IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider)
        {
            actionDescriptorCollectionProvider = _actionDescriptorCollectionProvider;
        }

        public static long? GetRouteModuleId(string url)
        {
            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }
            var route = actionDescriptorCollectionProvider.ActionDescriptors.Items
            .FirstOrDefault
            (
                ad => ad.AttributeRouteInfo != null
                && ad.EndpointMetadata.Any(w => w.GetType().Name == "RouteInfoAttribute")
                && ad.AttributeRouteInfo.Template == url
            );

            if (route != null)
            {
                var attribute = route.EndpointMetadata.FirstOrDefault(w => w.GetType().Name == "RouteInfoAttribute");

                var moduleIdString = attribute.GetType().GetProperty("ModuleId").GetValue(attribute).ToString();
                if (long.TryParse(moduleIdString, out var moduleId))
                {
                    return moduleId;
                }
                //if (String.IsNullOrEmpty(moduleIdString) && long.TryParse(moduleIdString, out var moduleId))
                //    return moduleId;
            }
            return null;
        }


    }
}
