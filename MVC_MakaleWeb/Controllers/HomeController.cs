using MakaleBLL;
using MakaleCommon;
using MakaleEntities;
using MakaleEntities.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using MVC_MakaleWeb.Models;
using System.Data.Entity;
using MVC_MakaleWeb.Filter;

namespace MVC_MakaleWeb.Controllers
{[Exc]
    public class HomeController : Controller
    {
        // GET: Home
        MakaleYonet my = new MakaleYonet();
        KategoriYonet ky = new KategoriYonet();
        KullaniciYonet kuly = new KullaniciYonet();
        BegeniYonet by=new BegeniYonet();
        public ActionResult Index()
        {
           // Test test = new Test();
            //  test.EkleTest();
            //test.UpdateTest();
            // test.DeleteTest();
            //test.YorumTest();

            //int i=0;   hata sayfasını test etmek için eklendi
            //int s=1/i;
            return View(my.Listele().Where(x=>x.Taslak!=true).ToList());
        }
        [Auth]
       public ActionResult Begendiklerim()
        {
            var query= by.ListQueryable().Include("Kullanici").Include("Makale").Where(x=>x.Kullanici.ID==SessionUser.Login.ID).Select(x=>x.Makale).Include("Kategori").Include("Kullanici").OrderByDescending(x=>x.DegistirmeTarihi);
               return View("Index",query.ToList());
        }
        public PartialViewResult kategoriPartial()
        {
            KategoriYonet ky = new KategoriYonet();
            List<Kategori> liste = ky.Listele();
            return PartialView("_PartialPagekat2", liste);
        }
        public ActionResult Kategori(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Kategori kat = ky.KategoriBul(id.Value);

            return View("Index", kat.makaleler);
        }
        public ActionResult EnBegenilenler()
        {
            return View("Index", my.Listele().OrderByDescending(x => x.BegeniSayisi).ToList());
        }

        public ActionResult SonYazilanlar()
        {
            return View("Index", my.Listele().OrderByDescending(x => x.DegistirmeTarihi).ToList());
        }

      public ActionResult Hakkımızda()
        {
            return View();
        }
        public ActionResult Iletisim()
        {
            return View();
        }
        public ActionResult Giris()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Giris(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                MakaleBLLSonuc<Kullanici> sonuc = kuly.LoginKontrol(model);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                SessionUser.Login = sonuc.nesne;
                Uygulama.login = sonuc.nesne.KullaniciAdi;
                return RedirectToAction("Index");
            }
            
       
            return View(model);
        }
        public ActionResult KayıtOl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KayıtOl(RegisterModel model)
        { 

            //Kullanıcı adı ve email varmı kontrol
            //kayıt işlemi yapılacak
            //aktivasyon maili gönderilcek 
            if (ModelState.IsValid)  //bu model geçerliyse yap
            {
                MakaleBLLSonuc<Kullanici> sonuc=kuly.KullaniciKaydet(model);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View();
                }
                else 
                { 
                    return RedirectToAction("KayitBasarili");
                
                }
              
            }

            return View(model);
        }
        public ActionResult KayitBasarili()  //aktivasyon maili göndermek için
        {
            return View();
        }
        
        public ActionResult HesapAktiflestir(Guid id)
        {
             MakaleBLLSonuc<Kullanici> sonuc = kuly.ActivateUser(id);
            if (sonuc.hatalar.Count>0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }
            return View();
        }
        public ActionResult Cikis()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult Error()
        {
            List<string> errors = new List<string>();
            if (TempData["hatalar"]!=null)
            {
                ViewBag.hatalar = TempData["hatalar"];
            }
           else
            {
                ViewBag.hatalar = errors;
            }
            
            return View();

        }
        [Auth]
        public ActionResult ProfilGoster( )
        {
            MakaleBLLSonuc<Kullanici>sonuc =kuly.KullaniciBul(SessionUser.Login.ID);
            if (sonuc.hatalar.Count>0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }

            return View(sonuc.nesne);
        }
        [Auth]
        public ActionResult ProfilDegistir()
        {
            
            MakaleBLLSonuc<Kullanici> sonuc = kuly.KullaniciBul(SessionUser.Login.ID);
            if (sonuc.hatalar.Count > 0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }
            return View( sonuc.nesne);
        }
       [ Auth ]
        [HttpPost]
        public ActionResult ProfilDegistir(Kullanici model, HttpPostedFileBase ProfilResim)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                if (ProfilResim != null && (ProfilResim.ContentType == "image/jpg" || ProfilResim.ContentType == "image/jpeg " || ProfilResim.ContentType == "image/PNG"))
                {//split ayırır parçalar uzantısını alabilmek için kullandık.
                    string dosya = $"user_{model.ID}.{ProfilResim.ContentType.Split('/')[1]}";
                    ProfilResim.SaveAs(Server.MapPath($"~/Resim/{dosya}"));
                    model.ProfilResimDosyaAdi = dosya;
                    
                }
                Uygulama.login = model.KullaniciAdi;

                MakaleBLLSonuc<Kullanici>sonuc= kuly.KullaniciUpdate(model);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                SessionUser.Login = sonuc.nesne;
              return RedirectToAction("ProfilGoster");
              

            }
            else
            {
                return View(model);
            }
            
        }
      [Auth]
      public ActionResult ProfilSil()
        {
            MakaleBLLSonuc<Kullanici>sonuc=kuly.KullaniciSil(SessionUser.Login.ID);
            if (sonuc.hatalar.Count>0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }
            Session.Clear();
            
            return RedirectToAction("Index");
        }
        public ActionResult YetkisizErisim()
        {
            return View();
        }
        public ActionResult HataliIslem()
        {
            return View();
        }
    }
}