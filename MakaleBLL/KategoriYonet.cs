using MakaleDAL;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class KategoriYonet
    {
        Repository<Kategori>rep_kat=new Repository<Kategori>();
       MakaleBLLSonuc<Kategori>sonuc=new MakaleBLLSonuc<Kategori>();
        public List<Kategori> Listele()
        {
            return rep_kat.Liste();
        }
       

        public Kategori KategoriBul(int id)
        {
           return rep_kat.Find(x=>x.ID==id);
        }
        public MakaleBLLSonuc<Kategori> KategoriSil(int id)
       
        {   Kategori kategori= rep_kat.Find(x => x.ID == id);
            Repository<Makale>rep_makale=new Repository<Makale>();
            Repository<Yorum> rep_yorum = new Repository<Yorum>();
            Repository<Begeni> rep_begeni = new Repository<Begeni>();
           
            //kategorinin makalelerini sil    
            //bunun yrine database de diagram ilişkilerinden delete 'i cascade yaparak bu kodları yazmayabilirsin ama bu tercih edilmez kontrolsüz silme işlemi olur.
            //aynı zamanda database oluşurken cascade yazabilirsin code first ile... database contextte yazıyoruz
            foreach (var item in kategori.makaleler.ToList())
            {
                //makalenin yorumlarını sil
                foreach (Yorum yorum in item.Yorumlar.ToList())
                {
                    rep_yorum.Delete(yorum);
                }
                //makalenin beğenilerini sil
                foreach (Begeni begeni in item.Begeniler.ToList())
                {
                    rep_begeni.Delete(begeni);
                }
                rep_makale.Delete(item);
            }
            rep_kat.Delete(sonuc.nesne);
            return sonuc;
        }

       

        public MakaleBLLSonuc<Kategori> KategoriEkle(Kategori model)
        {
           sonuc.nesne=rep_kat.Find(x=>x.Baslik==model.Baslik);
            if (sonuc.nesne!=null)
            {
                sonuc.hatalar.Add("Bu kategori kayıtlı");
            }
            else
            {
                if (rep_kat.Insert(model) < 1)
                {
                    sonuc.hatalar.Add("kategori kaydedilemedi");
                }
            }
            return sonuc;
        }

        public MakaleBLLSonuc<Kategori> KategoriUpdate(Kategori model)
        {
            sonuc.nesne=rep_kat.Find(x=>x.ID==model.ID);
            Kategori kategori=rep_kat.Find(x=>x.Baslik==model.Baslik && x.ID==model.ID);
            if (sonuc.nesne!=null && kategori==null)
            {
                sonuc.nesne.Baslik = model.Baslik;
                sonuc.nesne.Aciklama = model.Aciklama;
                if (rep_kat.Update(sonuc.nesne) < 1)
                {
                    sonuc.hatalar.Add("Kategori Güncellenemedi");
                }
            }
            else
            {
                if (kategori!=null)
                {
                    sonuc.hatalar.Add("Bu kategori zaten kayıtlı");
                }
                else
                {
                    sonuc.hatalar.Add("kategori bulunamadı");
                }
               
            }
            return sonuc;
        }
    }
}
