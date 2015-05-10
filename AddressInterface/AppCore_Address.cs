namespace AddressInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AppCore_Address
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Company { get; set; }

        [Required]
        [StringLength(150)]
        public string AddressLine1 { get; set; }

        [StringLength(150)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        public int StateId { get; set; }

        [Required]
        [StringLength(50)]
        public string Zip { get; set; }

        public virtual AppCore_State AppCore_State { get; set; }
    }
}
