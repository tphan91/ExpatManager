using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using System.Reflection;
using ExpatManager.Models;
using ExpatManager.DAL;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Web.Mvc;

namespace ExpatManager
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "AgreementPayment",
            //    "{controller}/{action}/{id}",             
           //     new { controller = "AgreementPayment", action = "CreateAgreementPayment", id = UrlParameter.Optional } // Defaults will also match "GetSmallImage"         
            //);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.Add(new Route("{controller}.mvc/{action}/{id}", new RouteValueDictionary(new { action = "Index", id = (string)null }), new MvcRouteHandler()));
        }

        protected void Application_Start()
        {
            /*
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .As<IController>();
            //Database.SetInitializer(new DropCreateDatabaseAlways<ExpatriateManagementContext>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ExpatriateManagementContext>());
            Database.SetInitializer<ExpatriateManagementContext>(new ExpatriateManagerInitializer());
            Database.SetInitializer<HRContext>(null);
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterType<EtrayServiceContractClient>().As<EtrayServiceContractClient>();
            builder.RegisterType<HomeController>().OnActivated(c => c.Instance.EtrayServiceContractClient = c.Context.Resolve<EtrayServiceContractClient>());
            _container = builder.Build();
            _containerProvider = new ContainerProvider(_container);
            ControllerBuilder.Current.SetControllerFactory(new AutofacControllerFactory(_containerProvider));
            
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);            
            */

            //Database.SetInitializer<ExpatriateManagementContext>(new ExpatriateManagerInitializer());
            Database.SetInitializer<ExpatriateManagementContext>(null);
            Database.SetInitializer<HRContext>(null);
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);


        }
    }
}