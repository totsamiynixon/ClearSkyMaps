using Readings.Domain.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readings.DAL.Implementations.Configurations
{
    public class ApplicationConfigurationConfiguration : IEntityTypeConfiguration<ApllicationConfiguration>
    {
        public void Configure(EntityTypeBuilder<ApllicationConfiguration> builder)
        {
            builder.HasKey(f => f.Id);
        }
    }
}
