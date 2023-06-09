﻿using MakaleDAL;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class Test
    {
        Repository<Kullanici> rep_kul = new Repository<Kullanici>();
        public Test()
        {
            // DatabaseContext db = new DatabaseContext();
            // db.kullanicilar.ToList();
            //db.Database.CreateIfNotExists();   //database yoksa cretae eder
            // db.kullanicilar.ToList();

            List<Kullanici> sonuc = rep_kul.Liste();
            List<Kullanici> liste = rep_kul.Liste(x=>x.Admin==true);

        }
        public void EkleTest()
        {
            rep_kul.Insert(new Kullanici() { Adi = "test", Soyadi = "test", Admin = false, Aktif = true, AktifGuid = Guid.NewGuid(), Email = "test", KayitTarihi = DateTime.Now, DegistirmeTarihi = DateTime.Now, DegistirenKullanici = "şüheda", KullaniciAdi = "test", Sifre = "test" });
        }
        public void UpdateTest()
        {
            Kullanici kul = rep_kul.Find(x => x.KullaniciAdi == "test");

            if (kul != null)
            {
                kul.Adi = "deneme";
                kul.Soyadi = "deneme";
                rep_kul.Update(kul);
            }
        }
        public void DeleteTest()
        {
            Kullanici kul = rep_kul.Find(x => x.KullaniciAdi == "test");

            if (kul != null)
            {
                rep_kul.Delete(kul);
            }
        }
        public void YorumTest()
        {
            Repository<Makale> rep_makale = new Repository<Makale>();
            Repository<Yorum> rep_yorum = new Repository<Yorum>();

            Makale m = rep_makale.Find(x => x.ID == 1);

            Kullanici k = rep_kul.Find(x => x.ID == 1);

            Yorum yorum = new Yorum()
            {
                Test = "Deneme yorumu",
                KayitTarihi = DateTime.Now,
                DegistirmeTarihi = DateTime.Now,
                DegistirenKullanici = "şüheda",
                Kullanici = k,
                Makale = m
            };
            rep_yorum.Insert(yorum);
        }
    }
}
