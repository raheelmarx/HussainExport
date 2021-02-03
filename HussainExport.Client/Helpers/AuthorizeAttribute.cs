using HussainExport.Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Helpers
{

    public class AuthorizeAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userToken = context.HttpContext.Session.GetString("token");
            if (context.HttpContext.Session.GetString("token") == null)
            {
                //context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                //{
                //    controller = "Home",
                //    action = "Index",
                //    returnurl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(context.HttpContext.Request)
                //}));
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "User",
                    action = "SignIn",
                    returnurl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(context.HttpContext.Request)
                }));
            }
            //else
            //{
            //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            //    {
            //        controller = "User",
            //        action = "SignIn",
            //        returnurl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(context.HttpContext.Request)
            //    }));
            //}
        }
    }
}
