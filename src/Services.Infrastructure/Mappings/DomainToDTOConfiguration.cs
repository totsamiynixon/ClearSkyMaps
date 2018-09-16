using AutoMapper;
using Domain;
using Services.DTO.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Infrastructure.Mappings
{
    public class DomainToDTOConfiguration : Profile
    {
        public DomainToDTOConfiguration()
        {
            CreateMap<Reading, SensorReadingDTO>();
        }
    }
}
