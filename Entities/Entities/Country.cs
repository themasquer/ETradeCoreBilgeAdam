using System.Collections.Generic;
using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class Country : RecordBase
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public List<City> Cities { get; set; }

        public List<UserDetail> UserDetails { get; set; }
    }
}
