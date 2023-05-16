using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakaleEntities;
using MakaleBLL;
using MVC_MakaleWeb.Filter;

namespace MVC_MakaleWeb.Controllers
{
    [Auth]
    [AuthAdmin]
    public class KullaniciController : Controller
    {
        KullaniciYonet ky=new KullaniciYonet();
        MakaleBLLSonuc<Kullanici> sonuc=new MakaleBLLSonuc<Kullanici>();

        // GET: Kullanici
        public ActionResult Index()
        {
            return View(ky.KullaniciListesi());
        }

        // GET: Kullanici/Details/5
        public ActionResult Details(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           sonuc= ky.KullaniciBul(id.Value);
            if (sonuc == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

       
        public ActionResult Create()
        {
          return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Kullanici kullanici)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                MakaleBLLSonuc<Kullanici> sonuc = ky.KullaniciKaydet(kullanici);

                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(kullanici);
                }
                return RedirectToAction("Index");
            }
            return View(kullanici);
        }

            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sonuc= ky.KullaniciBul(id.Value);
            if (sonuc.nesne == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Kullanici kullanici)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                MakaleBLLSonuc<Kullanici> sonuc = ky.KullaniciKaydet(kullanici);
                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(kullanici);
                }
                return RedirectToAction("Index");
            }
            return View(kullanici);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          sonuc= ky.KullaniciBul(id.Value);
            if (sonuc.nesne == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelState.Remove("BegeniSayisi");
           

            sonuc = ky.KullaniciSil(id);
           
            return RedirectToAction("Index");
        }

    }
}
