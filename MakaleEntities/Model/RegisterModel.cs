using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleEntities.Model
{
    public class RegisterModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(20)]
        public string KullaniciAdi { get; set; }
        [DisplayName("Email"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(50),EmailAddress(ErrorMessage ="{0} için geçerli bir email eposta adresi giriiniz.")]
        public string Email { get; set;}

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(20)]
        public string Sifre { get; set; }

        [DisplayName("Şifre2"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(20),Compare(nameof(Sifre),ErrorMessage =  "{0} ile {1} uyuşmuyor")]
        public string Sifre2  { get; set; }
    }
}
