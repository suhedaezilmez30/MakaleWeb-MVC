using MakaleCommon;
using MakaleDAL;
using MakaleEntities;
using MakaleEntities.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class KullaniciYonet
    {
        Repository<Kullanici> rep_kul = new Repository<Kullanici>();
        MakaleBLLSonuc<Kullanici> sonuc = new MakaleBLLSonuc<Kullanici>();
        public MakaleBLLSonuc<Kullanici> ActivateUser(Guid id)
        {
            MakaleBLLSonuc<Kullanici> sonuc = new MakaleBLLSonuc<Kullanici>();
            sonuc.nesne = rep_kul.Find(x => x.AktifGuid == id);
            if (sonuc.nesne != null)
            {
                if (sonuc.nesne.Aktif)
                {
                    sonuc.hatalar.Add("kullanıcı zaten daha önce aktif edilmiştir.");

                }
                else
                {
                    sonuc.nesne.Aktif = true;
                    rep_kul.Update(sonuc.nesne);
                }

            }
            else
            {
                sonuc.hatalar.Add("aktifleştirilecek kullanıcı bulunamadı");
            }
            return sonuc;
        }

        public MakaleBLLSonuc<Kullanici> KullaniciBul(int id)
        {
            
            sonuc.nesne = rep_kul.Find(x => x.ID == id);
            if (sonuc.nesne == null)
            {
                sonuc.hatalar.Add("kullanıcı bulunamadı");
            }
           
            return sonuc;
        }
        
        public MakaleBLLSonuc<Kullanici> KullaniciKaydet(RegisterModel model)
        {//bir modeli başka bir modele dönüştürme
            Kullanici nesne= new Kullanici();      
            nesne.Email=model.Email;
            nesne.KullaniciAdi = model.KullaniciAdi;

            sonuc = KullaniciKontrol(nesne);
            if (sonuc.hatalar.Count > 0)
            {
                sonuc.nesne = nesne;
                return sonuc;

            }
            else
            {
                int islemsonuc = rep_kul.Insert(new Kullanici()
                {
                    KullaniciAdi = model.KullaniciAdi,
                    Email = model.Email,
                    Sifre = model.Sifre,
                    AktifGuid = Guid.NewGuid(),
                    ProfilResimDosyaAdi="user_1.jpg"
                    

                });
                if (islemsonuc > 0)
                {
                    sonuc.nesne = rep_kul.Find(X => X.KullaniciAdi == model.KullaniciAdi && X.Email == model.Email);
                    string siteUrl = ConfigHelper.Get<string>("SiteRootUri");
                    string aktiveUrl = $"{siteUrl}/Home/HesapAktiflestir/{sonuc.nesne.AktifGuid}";
                    string body = $"Merhaba {sonuc.nesne.Adi}{sonuc.nesne.Soyadi} <br/> Hesabınızı aktifleştirmek için <a href='{aktiveUrl}' target='_blank'>tıklayınız</a>";
                    MailHelper.SendMail(body, sonuc.nesne.Email, "Hesap aktifleştirme");


                }
                return sonuc;
            }
           


        }
        public MakaleBLLSonuc<Kullanici> KullaniciKaydet(Kullanici kullanici)
        {
            sonuc = KullaniciKontrol(kullanici);
            if (sonuc.hatalar.Count > 0)
            {
                sonuc.nesne = kullanici;
                return sonuc;

            }
            else
            {
                sonuc.nesne = rep_kul.Find(x => x.ID == kullanici.ID);
                sonuc.nesne.Adi = kullanici.Adi;
                sonuc.nesne.Soyadi = kullanici.Soyadi;
                sonuc.nesne.Email = kullanici.Email;
                sonuc.nesne.KullaniciAdi = kullanici.KullaniciAdi;
                sonuc.nesne.Sifre = kullanici.Sifre;

                return sonuc;

            }
                
       }

        public MakaleBLLSonuc<Kullanici> KullaniciUpdate(Kullanici model)
        {

            sonuc = KullaniciKontrol(model);      
            if (sonuc.hatalar.Count>0)
            {
                sonuc.nesne = model;
                return sonuc;

            }       
            else
            {
                sonuc.nesne = rep_kul.Find(x => x.ID == model.ID);
                sonuc.nesne.Adi = model.Adi;
                sonuc.nesne.Soyadi = model.Soyadi;
                sonuc.nesne.Email = model.Email;
                sonuc.nesne.KullaniciAdi = model.KullaniciAdi;
                sonuc.nesne.Sifre = model.Sifre;
                sonuc.nesne.ProfilResimDosyaAdi = model.ProfilResimDosyaAdi;
               
                
                if (rep_kul.Update(sonuc.nesne) < 1)
                {
                    sonuc.hatalar.Add("profil bilgileri güncellenemedi.");

                }
                return sonuc;
            }
           
        }
        public MakaleBLLSonuc<Kullanici> KullaniciKontrol(Kullanici kullanici)
        {
            Kullanici k1=rep_kul.Find(x=>x.Email==kullanici.Email);
            Kullanici k2 = rep_kul.Find(x => x.KullaniciAdi == kullanici.KullaniciAdi);

            if (k1 !=null && k1.ID !=kullanici.ID)
            {
                sonuc.hatalar.Add("bu email adresi kayıtlı");
            }
            if (k2 !=null && k2.ID !=kullanici.ID)
            {
                sonuc.hatalar.Add("Bu kullanıcı adı kayıtlı");
            }
            return sonuc;
        }

        public MakaleBLLSonuc<Kullanici> LoginKontrol(LoginModel model)
            {
                MakaleBLLSonuc<Kullanici> sonuc = new MakaleBLLSonuc<Kullanici>();
                sonuc.nesne = rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi && x.Sifre == model.Sifre);
                if (sonuc.nesne == null)
                {
                    sonuc.hatalar.Add("kullanıcı adı yada şifre hatalı");
                }
                else
                {
                    if (!sonuc.nesne.Aktif)
                    {
                        sonuc.hatalar.Add("kullanıcı aktifleştirilememiştir.lütfen mail kutunuzu kontrol ediniz");
                    }

                }
                return sonuc;
            }

        public MakaleBLLSonuc<Kullanici> KullaniciSil(int id)
        {
            //Kullanici kullanici1 = rep_kul.Find(x => x.ID == id);
            //Repository<Makale> rep_makale = new Repository<Makale>();
            //Repository<Yorum> rep_yorum = new Repository<Yorum>();
            //Repository<Begeni> rep_begeni = new Repository<Begeni>();
            //foreach (var item in kullanici1.makaleler.ToList())
            //{
            //    //makalenin yorumlarını sil
            //    foreach (Yorum yorum in item.Yorumlar.ToList())
            //    {
            //        rep_yorum.Delete(yorum);
            //    }
            //    //makalenin beğenilerini sil
            //    foreach (Begeni begeni in item.Begeniler.ToList())
            //    {
            //        rep_begeni.Delete(begeni);
            //    }
            //    rep_makale.Delete(item);
            //}
            //rep_kul.Delete(sonuc.nesne);
            //return sonuc;

            Kullanici kullanici = rep_kul.Find(x => x.ID == id);
            if (kullanici != null)
            {
                if (rep_kul.Delete(kullanici) < 1)
                {
                    sonuc.hatalar.Add("Kullanıcı silinemedi");
                }
            }
            else
            {
                sonuc.hatalar.Add("kullanıcı bulunamadı");
            }
            return sonuc;
        }
        public List<Kullanici> KullaniciListesi()
        {
            return rep_kul.Liste();
        }


    }
}

