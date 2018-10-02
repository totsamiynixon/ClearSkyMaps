using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations.Configurations
{
    public class SensorConfiguration : EntityTypeConfiguration<Sensor>
    {
        public SensorConfiguration()
        {
            Property(c => c.TrackingKey)
             .HasMaxLength(450)
             .IsRequired()
             .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Sensor_Key")));
        }
    }
}
