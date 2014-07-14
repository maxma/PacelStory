using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;

namespace PacelStory
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes   web api 2 arrtibute routing
            config.MapHttpAttributeRoutes();

            // 传统路由
            config.Routes.MapHttpRoute
                (
                name: "SpecifiedCustomer",
                routeTemplate: "api/{controller}/SpecifiedCustomer/{customerId}"
                );

           
            config.Routes.MapHttpRoute
                (
                name: "SpecifiedCommunityById",
                routeTemplate: "api/{controller}/SpecifiedCommunityById/{customerId}"
                );

            config.Routes.MapHttpRoute
                (
                name: "SpecifiedCommunityByMobile",
                routeTemplate: "api/{controller}/SpecifiedCommunityByMobile/{mobile}"
                );
            

            config.Routes.MapHttpRoute
                (
                name: "CheckCustomerType2",
                routeTemplate: "api/{controller}/CheckCustomerType2/{mobile}"
                );

            config.Routes.MapHttpRoute
                (
                name: "GetGroupNames",
                routeTemplate: "api/{controller}/GetGroupNames/{wuyeMobile}"
                );

            config.Routes.MapHttpRoute
                (
                name: "SignPacel",
                routeTemplate: "api/{controller}/SignPacel/{pacelId}/{customerId}"
                );

            config.Routes.MapHttpRoute
                (
                name: "CheckCampOwnerCode",
                routeTemplate: "api/{controller}/CheckCampOwnerCode/{username}/{pwd}"
                );

            config.Routes.MapHttpRoute
                (
                name: "UnSigned",
                routeTemplate: "api/{controller}/{action}/{customerId}/{pageNumber}"
                );

            //config.Routes.MapHttpRoute
            //    (
            //    name: "SpecifiedCustomer",
            //    routeTemplate: "api/Customer/SpecifiedCustomer/{customerId}"
            //    );

            config.Routes.MapHttpRoute
                (
                name: "SpecifiedPacel",
                routeTemplate: "api/{controller}/{action}/{pacelId}"
                );




            config.Routes.MapHttpRoute
                (
                name: "CreateCustomer",
                routeTemplate: "api/{controller}/{action}"
                );

            config.Routes.MapHttpRoute
                (
                name: "DELETE",
                routeTemplate: "api/{controller}/{action}/{id}"
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );



            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
