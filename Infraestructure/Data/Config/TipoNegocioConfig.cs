using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Config
{
    public class TipoNegocioConfig : IEntityTypeConfiguration<TipoNegocio>
    {
        public void Configure(EntityTypeBuilder<TipoNegocio> builder)
        {
            builder.ToTable("TiposNegocio");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nombre)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(c => c.Activo);
        }
    }
}
