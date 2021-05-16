using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCore.Records.Bases;

namespace Entities.Entities
{
    public class Role : RecordBase
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Role ile User arasında 1 to Many ilişki olduğu için Role'de List of Users, User'da da RoleId ve Role tanımlanmalıdır. 
        public List<User> Users { get; set; }
    }
}
