namespace AddressInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AppCore_State
    {
        public AppCore_State()
        {
            AppCore_Address = new HashSet<AppCore_Address>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Abbreviation { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<AppCore_Address> AppCore_Address { get; set; }
    }
}
