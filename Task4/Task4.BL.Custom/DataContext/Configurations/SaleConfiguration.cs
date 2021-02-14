using Task4.Model.Models;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task4.BL.Custom.DataContext.Configurations
{
    public class SaleConfiguration : EntityTypeConfiguration<Sale>
    {
        public SaleConfiguration()
        {
            this
                .ToTable("Sales")
                .HasKey(x => x.Id);
            this
                .Property(x => x.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this
                .Property(x => x.Date)
                .HasColumnType("datetime")
                .IsRequired();
            this
                .Property(x => x.Client)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();
            this
                .Property(x => x.Cost)
                .HasColumnType("money")
                .IsRequired();
            //this
            //    .Property(x => x.Manager.SecondName)
            //    .HasColumnType("varchar")
            //    .HasMaxLength(50)
            //    .IsRequired();
        }
    }
}