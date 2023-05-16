using MakaleBLL;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MVC_MakaleWeb.Models
{
    public class CasheHelper
    {
      public static List<Kategori> KategoriCache()
        {
         var Kategoriler= WebCache.Get("kat-cache");
            if (Kategoriler==null)
            {//ilk çağırdığımzda cache atıcAK SONRA HER ÇAĞIRDIĞIMIZDA CACHE DEN OKUYACAK
                KategoriYonet ky=new KategoriYonet();
                Kategoriler=ky.Listele();
                WebCache.Set("kat-cache",Kategoriler,20,true);
            }
            return Kategoriler;
        }
        public static void CacheTemizle()
        {
            WebCache.Remove("kat-cache");
        }
    }
}