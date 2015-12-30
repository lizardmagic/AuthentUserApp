using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RCInvigilator
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Sessions",
				url: "Sessions",
				defaults: new { controller = "Sessions", action = "Index" }
			);

			routes.MapRoute(
				name: "Exams",
				url: "Exams",
				defaults: new { controller = "Exams", action = "Index", id = UrlParameter.Optional}
			);

			routes.MapRoute(
				name: "Students",
				url: "Students",
				defaults: new { controller = "Students", action = "Index", id = UrlParameter.Optional}
			);

			routes.MapRoute(
				name: "Invigilators",
				url: "Invigilators",
				defaults: new { controller = "Invigilators", action = "Index", id = UrlParameter.Optional}
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
			);
        }
    }
}
