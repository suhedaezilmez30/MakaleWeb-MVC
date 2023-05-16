﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakaleBLL;
using MakaleEntities;
using MVC_MakaleWeb.Filter;
using MVC_MakaleWeb.Models;

namespace MVC_MakaleWeb.Controllers
{[Exc]
    public class YorumController : Controller
    {
        // GET: Yorum

        public ActionResult YorumGoster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MakaleYonet my =new MakaleYonet();
            //value dememmizin nedeni null olabilir dediğimiz için
            Makale makale= my.MakaleBul(id.Value);
            if (makale==null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialPageYorumlar",makale.Yorumlar);
        }
        [Auth]
        [HttpPost]
        public ActionResult YorumGuncelle(int? id,string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YorumYonet yy=new YorumYonet();
            Yorum yorum=yy.YorumBul(id.Value);
            if (yorum==null)
            {
                return HttpNotFound();

            }
            yorum.Test=text;
           if(yy.YorumUpdate(yorum) > 0)
            {
                return Json(new {hata=false},JsonRequestBehavior.AllowGet);
            }
           return Json(new {hata=true},JsonRequestBehavior.AllowGet);
        }
        [Auth]
        public ActionResult YorumSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YorumYonet yy = new YorumYonet();
            Yorum yorum = yy.YorumBul(id.Value);
            if (yorum == null)
            {
                return HttpNotFound();

            }

            if (yy.YorumSil(yorum) > 0)
            {
                return Json(new { hata = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { hata = true }, JsonRequestBehavior.AllowGet);
        }
        [Auth]
        public ActionResult YorumEkle(Yorum nesne,int? id){
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                MakaleYonet my = new MakaleYonet();
                Makale makale = my.MakaleBul(id.Value);
                if (makale == null)
                {
                    return HttpNotFound();

                }
                nesne.Makale=makale;
                nesne.Kullanici=SessionUser.Login;
                YorumYonet yy=new YorumYonet();
                if (yy.YorumEkle(nesne) > 0)
                {
                    return Json(new { hata = false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { hata = true }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}