using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Net.WebRequestMethods;

namespace MVC_MakaleWeb.Models
{
    public static class SessionUser
    {
        public static Kullanici Login 
        {
            //sessiondaki değeri daha rahat kullanmak için her yerde örnekleme gerek kalmicak
            get
            {
              if (HttpContext.Current.Session["login"] != null)
                {
                    return HttpContext.Current.Session["login"] as Kullanici;
                }
              return null;
            }
            set
            {
                HttpContext.Current.Session["login"]=value;
            } 
        
        }

    }
}