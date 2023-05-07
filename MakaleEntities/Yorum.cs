using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleEntities
{
    [Table("Yorum")]
    public class Yorum:BaseClass
    {
        [StringLength(250)]
        public String Test { get; set; }

        public virtual Makale Makale { get; set; }  //yorumun ait olduğu bir makale var. makale ile ilişkisini göstermek adına virtual tanımladık
        public virtual Kullanici Kullanici { get; set; }
    }
}
