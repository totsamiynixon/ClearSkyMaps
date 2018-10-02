using DAL.Implementations.Contexts;
using DAL.Intarfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Implementations
{
    public class DatabaseMigrator : IDatabaseMigrator
    {
        private readonly DataContext _context;
        public DatabaseMigrator(DataContext context)
        {
            _context = context;
        }
        public void Migrate()
        {
            _context.Database.Migrate();
        }
    }
}
