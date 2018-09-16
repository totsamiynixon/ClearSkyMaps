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
    public class DTOToDomainConfiguration : Profile
    {
        public DTOToDomainConfiguration()
        {
            CreateMap<SaveReadingDTO, Reading>();
        }
    }
}
