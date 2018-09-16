using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Admin
{
    public class ApllicationConfiguration : IEntity
    {
        public int Id { get; set; }

        public bool EnableEmulation { get; set; }
    }
}
