using System.ComponentModel.DataAnnotations;
using AppCore.Records.Bases;

namespace Entities.Entities
{
    public class User : RecordBase
    {
        [Required]
        //[StringLength(30)] // StringLength kullanılması daha uygun entity'ler için
        [MinLength(3)]
        [MaxLength(30)]
        public string UserName { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        public bool Active { get; set; }

        // Role ile User arasında 1 to Many ilişki olduğu için Role'de List of Users, User'da da RoleId ve Role tanımlanmalıdır. 
        public int RoleId { get; set; }
        public Role Role { get; set; }

        // UserDetail ile User arasında 1 to 1 ilişki olduğu için User'da UserDetailId ve UserDetail, UserDetail'da da User tanımlanmalıdır. 
        public int UserDetailId { get; set; }
        public UserDetail UserDetail { get; set; }
    }
}
