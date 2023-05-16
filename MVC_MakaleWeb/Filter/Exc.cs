using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_MakaleWeb.Filter
{
    public class Exc : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Controller.TempData["hatalar"]=filterContext.Exception;
            filterContext.ExceptionHandled = true; //kendimiz yönlendircez o yüzden true yaptık
            filterContext.Result=new RedirectResult("/Home/HataliIslem");
        }
    }
}