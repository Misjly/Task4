using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Task4.DAL.Models;

namespace Task4.BL.Custom.DataContext.Configurations
{
    public class ManagerConfiguration : EntityTypeConfiguration<Manager>
    {
        public ManagerConfiguration()
        {

            this.ToTable("Managers")
                .HasKey(x => x.Id)
                .HasMany(p => p.Sales)
                .WithRequired(p => p.Manager);
            this
                .HasIndex(x => x.Id)
                .IsUnique();
            this
                .Property(x => x.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this
                .Property(x => x.SecondName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
