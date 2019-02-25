using Readings.DAL.Intarfaces;
using Readings.Domain.Admin;
using Microsoft.EntityFrameworkCore;
using Readings.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readings.Services.Implementations
{
    public class ApplicationConfigurationService : IApplicationConfigurationService
    {
        private static bool Value { get; set; }
        private static DateTime LastUpdate { get; set; }

        private readonly IDataContext _db;
        private readonly DbSet<ApllicationConfiguration> _apllicationConfigurationRepository;

        public ApplicationConfigurationService(IDataContext context)
        {
            _db = context;
            _apllicationConfigurationRepository = _db.GetRepository<ApllicationConfiguration>();
        }
        public async Task<bool> IsSimulatorEnabledAsync()
        {

            if (LastUpdate == default(DateTime))
            {
                LastUpdate = DateTime.UtcNow;
            }
            if ((DateTime.UtcNow - LastUpdate).Minutes > 5)
            {
                var configuration = await _apllicationConfigurationRepository.FirstOrDefaultAsync();
                if(configuration == null)
                {
                    Value = false;
                }
                else
                {
                    Value = configuration.EnableEmulation;
                }
                LastUpdate = DateTime.UtcNow;
            }
            return Value;
        }


    }
}
