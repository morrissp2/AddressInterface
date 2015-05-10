namespace AddressInterface
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AddressContext : DbContext
    {
        public AddressContext()
            : base("name=AddressContext")
        {
        }

        public virtual DbSet<AppCore_Address> AppCore_Address { get; set; }
        public virtual DbSet<AppCore_State> AppCore_State { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppCore_State>()
                .HasMany(e => e.AppCore_Address)
                .WithRequired(e => e.AppCore_State)
                .HasForeignKey(e => e.StateId)
                .WillCascadeOnDelete(false);
        }
    }
}
