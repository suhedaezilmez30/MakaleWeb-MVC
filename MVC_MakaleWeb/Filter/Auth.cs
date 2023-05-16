using MVC_MakaleWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_MakaleWeb.Filter
{
    public class Auth : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionUser.Login==null)
            {
                filterContext.Result=new RedirectResult("/Home/Giris");
            }
        }
    }
}