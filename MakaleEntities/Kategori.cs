﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleEntities
{
    [Table("Kategori")]
    public class Kategori:BaseClass
    {
        [Required,StringLength(50)]
        public string Baslik { get; set; }
        [StringLength(150)]
        public string Aciklama { get; set; }

        public virtual List<Makale>makaleler { get; set; }

        public Kategori()
        {
            makaleler=new List<Makale>();
            
        }

    }
}
