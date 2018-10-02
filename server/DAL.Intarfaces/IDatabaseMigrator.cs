using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Intarfaces
{
    public interface IDatabaseMigrator
    {
        void Migrate();
    }
}
