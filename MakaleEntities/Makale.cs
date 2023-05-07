﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MakaleEntities
{
    [Table("Makale")]
    public class Makale : BaseClass
    {
        [Required,StringLength(100)]
        public string Baslik { get; set; }
        [Required]
        public string Icerik { get; set; }
        
        public bool Taslak { get; set; }
        public int BegeniSayisi { get; set; }
        public virtual List<Yorum> Yorumlar { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual Kategori Kategori { get; set; }

        public virtual List<Begeni> Begeniler { get; set; }

        public Makale()
        {
            Yorumlar = new List<Yorum>();
            Begeniler = new List<Begeni>();    
        }

    }
}
