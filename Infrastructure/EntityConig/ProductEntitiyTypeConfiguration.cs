using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConig
{
    public class ProductEntitiyTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(e => e.Id).IsRequired();
            builder.Property(e=>e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e=>e.Price).HasColumnType("decimal");
            builder.Property(e => e.PictureUrl).IsRequired();
            builder.HasOne(e => e.productBrand).WithMany(e=>e.Products).HasForeignKey(e => e.ProductBrandId);
            builder.HasOne(e => e.ProductType).WithMany(e=>e.Products).HasForeignKey(e => e.ProductTypeId);
           
        }
    }
}
