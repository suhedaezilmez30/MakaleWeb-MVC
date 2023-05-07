using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleEntities
{
    [Table("Kullanıcı")]
    public class Kullanici:BaseClass
    {
        [StringLength(20),DisplayName("Ad")]
        public string Adi { get; set; }
        [StringLength(20),DisplayName("Soyadı")]
        public string Soyadi { get; set; }
        [Required, StringLength(20),DisplayName("kullanıcı adı")]
        public string KullaniciAdi { get; set; }
        [Required, StringLength(50)]
        public string Email { get; set; }
        [Required, StringLength(20) ,DisplayName("şifre")]
        public string Sifre { get; set; }
        [StringLength (30)]
        public string ProfilResimDosyaAdi { get; set; }
        public bool Aktif { get; set; }
        [Required]
        public Guid AktifGuid { get; set; }
        public bool Admin { get; set; }

        public virtual List<Makale>makaleler { get; set; }
        public virtual List<Yorum>yorumlar { get; set; }
        public virtual List<Begeni>begeniler { get; set; }

    }
}
