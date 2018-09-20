using Services.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PDK : Attribute
    {
        public double PDKValue { get; set; }
        public LevelOfDanger LevelOfDanger { get; set; }
        public PDK(double pdk, LevelOfDanger levelOfDanger)
        {
            PDKValue = pdk;
            LevelOfDanger = LevelOfDanger;
        }
    }
}
