using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Workers.Data.Entity
{
    public class Worker
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Isim girilmesi zorunludur"),Display(Name="Ad")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyisim girilmesi zorunludur"), Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Telefon girilmesi zorunludur"), Display(Name = "Telefon")]
        public string Phone { get; set; }
        public int ManagerId { get; set; }
        [ForeignKey("ManagerId"), Display(Name = "Yönetici")]
        public virtual Worker Manager { get; set; }

        public int DeptId { get; set; }
        [ForeignKey("DeptId"), Display(Name = "Departman")]
        public virtual Department Department { get; set; }
    }
}
