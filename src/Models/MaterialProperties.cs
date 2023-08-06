using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SilentGearLookup.Models;

namespace SilentGearLookup.Models
{
    internal class MaterialProperties
    {

        public MaterialProperties(MaterialAvailability? availability, MaterialName? name, MaterialStats? stats)
        {
            Availability = availability;
            Name = name;
            Stats = stats;
        }

        public MaterialAvailability? Availability { get; set; }
        public MaterialName? Name { get; set; }
        public MaterialStats? Stats { get; set; }
    }
}
