using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleDAL
{
    public class VeriTabanıOlusturucu:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            Kullanici admin = new Kullanici()
            {
                Adi = "şüheda",
                Soyadi = "ezilmez",
                Email = "bilisimshda@gmail.com",
                Admin = true,
                Aktif = true,
                KullaniciAdi="suhedaezilmez",
                Sifre="123",
                AktifGuid=Guid.NewGuid(),
                KayitTarihi=DateTime.Now,
                DegistirmeTarihi=DateTime.Now.AddMinutes(10),
                DegistirenKullanici="suheda",    
            };
            context.kullanicilar.Add(admin);

            for (int i = 1; i < 6; i++)
            {
                Kullanici k = new Kullanici()
                {
                    Adi = FakeData.NameData.GetFirstName(),
                    Soyadi = FakeData.NameData.GetSurname(),
                    Admin = false,
                    Aktif = true,
                    Email = FakeData.NetworkData.GetEmail(),
                    AktifGuid = Guid.NewGuid(),
                    KullaniciAdi = FakeData.NameData.GetFirstName() + i.ToString(),
                    Sifre = $"123{i}",
                    KayitTarihi = DateTime.Now,
                    DegistirmeTarihi = DateTime.Now.AddMinutes(10),
                    DegistirenKullanici = FakeData.NameData.GetFirstName() + i.ToString(),
                     
                };

                context.kullanicilar.Add(k);

            }
            context.SaveChanges();
            List<Kullanici> kullanicilar = context.kullanicilar.ToList();

            for (int i = 0; i < 6; i++)
            {
                //kategori ekle

                Kategori kat = new Kategori()
                {
                    Baslik=FakeData.PlaceData.GetStreetName(),
                    Aciklama=FakeData.PlaceData.GetAddress(),
                    KayitTarihi=DateTime.Now,
                    DegistirmeTarihi=DateTime.Now,
                    DegistirenKullanici=admin.KullaniciAdi,
                };
                context.kategoriler.Add(kat);
               
                for (int j= 0; j < 6; j++)
                {
                    Kullanici kullanici = kullanicilar[FakeData.NumberData.GetNumber(0, 5)];
                    //makale ekle
                    Makale mak = new Makale()
                    {
                        Baslik = FakeData.TextData.GetAlphabetical(7),
                        Icerik = FakeData.TextData.GetSentences(3),
                        Taslak = false,
                        BegeniSayisi = FakeData.NumberData.GetNumber(0, 5),
                        KayitTarihi = DateTime.Now,
                        DegistirmeTarihi = DateTime.Now.AddDays(2),
                        DegistirenKullanici = kullanici.KullaniciAdi,
                        Kullanici = kullanici                      

                    };

                    kat.makaleler.Add(mak);

                    //YORUM EKLE
                    for (int z = 0; z < 6; z++)
                    {
                        Kullanici Yorum_Kullanici = kullanicilar[FakeData.NumberData.GetNumber(0, 5)];
                        Yorum yorum = new Yorum()
                        {
                            Test = FakeData.TextData.GetSentences(2),
                            KayitTarihi = DateTime.Now.AddDays(-1),
                            DegistirenKullanici = Yorum_Kullanici.KullaniciAdi,
                            DegistirmeTarihi = DateTime.Now,
                            Kullanici = Yorum_Kullanici
                            
                        };
                        //yorum.Makale = mak;
                        //context.Yorumlar.Add(yorum);
                        mak.Yorumlar.Add(yorum);


                        for (int x = 0; x < mak.BegeniSayisi; x++)
                        {
                            Kullanici begen_kullanici = kullanicilar[FakeData.NumberData.GetNumber(0, 5)];
                            Begeni begen = new Begeni() 
                            {
                                Kullanici=begen_kullanici
                            };
                            mak.Begeniler.Add(begen);
                        }
                    }

                   
                    ///beğeni ekleme
                    for (int x = 0; x <mak.BegeniSayisi; x++)
                    {

                        Begeni begen = new Begeni()
                        {
                            Kullanici = kullanicilar[x]
                        };
                        mak.Begeniler.Add(begen);
                    }
                   
                }
                context.SaveChanges();
            }

        }
    }
}
