using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;

namespace WebApi {
    class Program {
        static void Main(string[] args) {
            string uri = "http://+:5501";
            WebApp.Start(uri, appBuilder => Configure(uri, appBuilder));
            Console.WriteLine("Web API endpoint started at ");
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }

        private static void Configure(string uri, IAppBuilder appBuilder) {
            // For now just use basic or windows authentication
            var listener = (HttpListener)appBuilder.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes = AuthenticationSchemes.Basic | AuthenticationSchemes.IntegratedWindowsAuthentication;

            // Configure endpoint:
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{key}",
            //    defaults: new { key = RouteParameter.Optional }
            //);


            appBuilder.UseWebApi(config);
        }
    }
}
