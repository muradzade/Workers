using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Workers.WebUI.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage ="Güncel şifrenin girilmesi zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name = "Güncel şifre")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifrenin girilmesi zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni şifre")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifrenin girilmesi zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni şifre doğrula")]
        [Compare("NewPassword",ErrorMessage ="Girilen şifreler aynı değil")]
        public string ConfirmNewPassword { get; set; }
    }
}
