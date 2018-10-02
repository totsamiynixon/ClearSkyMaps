using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations.Configurations
{
    public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
    {
        public void Configure(EntityTypeBuilder<Sensor> builder)
        {
            builder.Property(c => c.TrackingKey)
             .HasMaxLength(450)
             .IsRequired();
            builder.HasIndex(i => i.Id);
            builder.HasIndex(i => i.TrackingKey);
            builder.HasKey(i => i.Id);
        }
    }
}
