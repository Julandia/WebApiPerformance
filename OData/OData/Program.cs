using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.OData.Edm;
using Microsoft.Owin.Hosting;
using OData.Model;
using Owin;

namespace OData {
    class Program {
        static void Main(string[] args) {
            string uri = "http://+:5500";
            WebApp.Start(uri, appBuilder => Configure(uri, appBuilder));
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }

        private static void Configure(string uri, IAppBuilder appBuilder) {
            // For now just use basic or windows authentication
            var listener = (HttpListener)appBuilder.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes = AuthenticationSchemes.Basic | AuthenticationSchemes.IntegratedWindowsAuthentication;

            // Configure endpoint:
            string pathBase = new Uri(uri.Replace("+", "localhost")).AbsolutePath;
            HttpConfiguration config = new HttpConfiguration();

            var conventions = ODataRoutingConventions.CreateDefault();
            //config.MapODataServiceRoute("odata", null, GetEdmModel(), new DefaultODataPathHandler(), conventions);
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: GetEdmModel());
            appBuilder.UseWebApi(config);
        }

        private static IEdmModel GetEdmModel() {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "Performance";
            builder.ContainerName = "DefaultContainer";

            builder.EntitySet<Greeting>("Greetings");
            builder.EntityType<Greeting>().Collection.Action("LongOnes").Returns<string>().Parameter<int>("length");
            var edmModel = builder.GetEdmModel() as EdmModel;

            return edmModel;
        }
    }
}
