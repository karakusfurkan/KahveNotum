using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KahveNotum.Entities
{
    [Table("KahveNotumUsers")]
    public class KahveNotumUser : MyEntityBase
    {
        [DisplayName("İsim"), StringLength(25)]
        public string Name { get; set; }

        [DisplayName("Soyisim"), StringLength(25)]
        public string SurName { get; set; }

        [DisplayName("Mail"), StringLength(70)]
        public string Email { get; set; }

        [DisplayName("Şifre"), StringLength(25)]
        public string Password { get; set; }

        [DisplayName("Kullanıcı Adı"), Required, StringLength(25)]
        public string Username { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }

        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }

        [StringLength(30),ScaffoldColumn(false)]  //user0.jpg for example
        public string ProfileImageFileName { get; set; }


        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }

        public List<Liked> Likes { get; set; }


    }
}
