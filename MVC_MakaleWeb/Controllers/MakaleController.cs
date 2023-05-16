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
using MVC_MakaleWeb.Models;
using System.Web.WebSockets;
using MVC_MakaleWeb.Filter;

namespace MVC_MakaleWeb.Controllers
{
    [Exc]
    public class MakaleController : Controller
    {
        MakaleYonet my=new MakaleYonet();
        KategoriYonet ky = new KategoriYonet();
        MakaleBLLSonuc<Makale>sonuc=new MakaleBLLSonuc<Makale>();

        [Auth]
        public ActionResult Index()
        {
            if (SessionUser.Login!=null)
            {
                return View(my.Listele().Where(x => x.Kullanici.ID == SessionUser.Login.ID));
            }
           return View(my.Listele());
        }

        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // GET: Makales/Create
        [Auth]
        public ActionResult Create()
        {
            ViewBag.Kategori = new SelectList(CasheHelper.KategoriCache(),"ID","Baslik"); //dropdawn listi dolduemak için
            return View();
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Makale makale)
        {
            makale.Kategori=ky.KategoriBul(makale.Kategori.ID);
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ViewBag.Kategori = new SelectList(CasheHelper.KategoriCache(), "ID", "Baslik", makale.Kategori.ID);
            if (ModelState.IsValid)
            {
                makale.Kullanici = SessionUser.Login;
                makale.Kategori=ky.KategoriBul(makale.Kategori.ID);
               sonuc=  my.makaleEkle(makale);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }
                return RedirectToAction("Index");
            }
            
            return View(makale);
        }

        [Auth]
        // GET: Makales/Edit/5 ---Update edilmeli
        public ActionResult Edit(int? id)
        {
            Makale makale = my.MakaleBul(id.Value);
            ViewBag.Kategori = new SelectList(CasheHelper.KategoriCache(), "ID", "Baslik",makale.Kategori.ID);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            if (makale == null)
            {
                return HttpNotFound();
            }
          
            return View(makale);
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Makale makale)
        {
           
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ViewBag.Kategori = new SelectList(CasheHelper.KategoriCache(), "ID", "Baslik", makale.Kategori.ID);
            if (ModelState.IsValid)
            {
                makale.Kategori = ky.KategoriBul(makale.Kategori.ID);
                sonuc= my.MakaleUpdate(makale);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }
                return RedirectToAction("Index");
            }
            return View(makale);
        }
        [Auth]
        // GET: Makales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            my.MakaleSil(id);
            return RedirectToAction("Index");
        }
        BegeniYonet by = new BegeniYonet();
        [HttpPost]   //post yazmak zorunda değiliz ama eğer geti varsa metodun onun çalışmaması için postu belirtmek daha güvenli
        [Auth]
        public ActionResult MakaleGetir(int[] mid)
        {           
            List<int>mliste=null;
            if (SessionUser.Login!=null && mid !=null)
            {
                mliste = by.liste().Where(x => x.Kullanici.ID == SessionUser.Login.ID && mid.Contains(x.Makale.ID)).Select(x => x.Makale.ID).ToList();              
            }
            return Json(new { liste = mliste });
        }
        [Auth]
        [HttpPost]
        public ActionResult MakaleBegen(int makaleId,bool begeni)
        {
         Begeni like =by.BegeniBul(makaleId,SessionUser.Login.ID);
            Makale makale= my.MakaleBul(makaleId);
            int sonuc=0;

            if (like!=null && begeni==false) //beğenmekten vazgeçiyoruz
            {
               sonuc= by.BegeniSil(like);

            }
            else if (like==null && begeni==true) //beğeniyorsam
            {
              sonuc=  by.BegeniEkle(new Begeni() {
                    Kullanici=SessionUser.Login,
                    Makale=makale              
                    });
            }
            if (sonuc>0) //sonuc  dan büyükse begenilmiş yada beğeni kaldırılmıştır bu yüzden sil yada ekle işlemi yapar
            {
                if (begeni) //begeni true ise begenilmiştir o yüzden beğeni sayısını arttırmalıyız
                {
                    makale.BegeniSayisi++;
                }
                else
                {
                    makale.BegeniSayisi--;
                }
                my.MakaleUpdate(makale);

                return Json(new {hata=false,begenisayisi=makale.BegeniSayisi});
            }
            else
            {
                return Json(new { hata=true, begenisayisi = makale.BegeniSayisi });
            }
        }
     
      
        public ActionResult MakaleGoster(int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
  
                return PartialView("_PartialPageMakaleGoster", makale);
           
                
            
        }
        [Auth]
        public ActionResult YorumGoster()
        {
            return View();
        }
    }
}
