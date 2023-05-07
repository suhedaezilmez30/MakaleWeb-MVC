using MakaleCommon;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace MakaleDAL
{
    public class Repository<T> : Singleton, IRepository<T> where T : class
    {
        private DbSet<T> dbset;
        public Repository()
        {
            dbset = db.Set<T>();
        }
        public int Delete(T nesne)
        {
            dbset.Remove( nesne );
            return db.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> kosul)
        {
           return dbset.FirstOrDefault(kosul);
        }

        public int Insert(T nesne)
        {
            dbset.Add( nesne );
            if (nesne is BaseClass)  //beğeni base clasdan kalıtılmadığı için bu şekilde yapıldı
            {
                BaseClass obj = nesne as BaseClass;
                DateTime tarih=DateTime.Now;
                obj.KayitTarihi = tarih;
                obj.DegistirmeTarihi = tarih;
                obj.DegistirenKullanici = Uygulama.login;
            }
          
            return db.SaveChanges();
        }

        public List<T> Liste()
        {
            return dbset.ToList();
        }

        public List<T> Liste(Expression<Func<T, bool>> kosul)
        {
            return dbset.Where(kosul).ToList();
        }

        public int Update(T nesne)
        {
            if (nesne is BaseClass)  //beğeni base clasdan kalıtılmadığı için bu şekilde yapıldı
            {
                BaseClass obj = nesne as BaseClass;
               
                obj.DegistirmeTarihi = DateTime.Now;
                obj.DegistirenKullanici = Uygulama.login;
            }
            return db.SaveChanges();
        }
    }
}
