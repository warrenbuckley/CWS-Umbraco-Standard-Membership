using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;

namespace CWSUmbracoStandardMembership.Code
{
    public class UmbracoStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //Register custom MVC route for user profile
            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "ProfilePage",                                                          // Route name
                "user/{profileURLtoCheck}",                                             // URL with parameters
                new { controller = "MemberProfile", action = "RenderMemberProfile" }    // Parameter defaults
            );
        }
    }
}