using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakaleBLL;
using MakaleDAL;
using MakaleEntities;
using MVC_MakaleWeb.Filter;
using MVC_MakaleWeb.Models;

namespace MVC_MakaleWeb.Controllers
{
    [Exc]
    [Auth]
    [AuthAdmin]
    public class KategoriController : Controller
    {
       
        KategoriYonet ky=new KategoriYonet();
        // GET: Kategoris
        public ActionResult Index()
        {
            return View(ky.Listele()); 
        }

        // GET: Kategoris/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // GET: Kategoris/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Kategori kategori)
        {
            
                ModelState.Remove("DegistirenKullanici");
                if (ModelState.IsValid)
                {
                  MakaleBLLSonuc<Kategori>sonuc=ky.KategoriEkle(kategori);
                  if(sonuc.hatalar.Count>0)
                    {
                       sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                       return View(kategori);
                     }
                  CasheHelper.CacheTemizle();
                 return RedirectToAction("Index");
                }
                return View(kategori);        
        }


        // GET: Kategoris/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Kategori kategori)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            { MakaleBLLSonuc<Kategori>sonuc=ky.KategoriUpdate(kategori);
                if (sonuc.hatalar.Count>0)
                {
                        sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                        return View(kategori);
                }
                CasheHelper.CacheTemizle();
                return RedirectToAction("Index");
            }
            return View(kategori);
        }

        // GET: Kategoris/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // POST: Kategoris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            ky.KategoriSil(id);
            CasheHelper.CacheTemizle();

            return RedirectToAction("Index");
        }

    }
}
