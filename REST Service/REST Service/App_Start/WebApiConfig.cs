﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;

namespace REST_Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Registration",
                routeTemplate: "register",
                defaults: new { controller = "register" }
            );
            config.Routes.MapHttpRoute(
                name: "Authentication",
                routeTemplate: "authenticate",
                defaults: new { controller = "login" }
            );
            config.Routes.MapHttpRoute(
                name: "ActionRoute",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            );
            config.Routes.MapHttpRoute(
                name: "GET Route",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { action = "Get", id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /*
             * For when the server needs to accept access tokens
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{accessToken}/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
             */

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

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
