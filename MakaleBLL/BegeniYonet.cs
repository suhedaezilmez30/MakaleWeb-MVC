using MakaleDAL;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
   public class BegeniYonet
    {
        Repository<Begeni>rep_begen=new Repository<Begeni>();
        public IQueryable<Begeni> ListQueryable()
        {
            return rep_begen.ListQueryable();
        }
        public List<Begeni> liste()
        {
            return rep_begen.Liste();
        }
        public Begeni BegeniBul(int mid,int kid)
        {
          return rep_begen.Find(x=>x.Makale.ID==mid && x.Kullanici.ID==kid);
        }
        public int BegeniEkle(Begeni begeni)
        {
            return rep_begen.Insert(begeni);
        }
        public int BegeniSil(Begeni begeni)
        {
            return rep_begen.Delete(begeni);
        }
    }
}
