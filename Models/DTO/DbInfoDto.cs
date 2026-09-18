using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class DbInfoDto
    {
        public int NrSeededUsers { get; set; } = 0;
        public int NrUnseededUsers { get; set; } = 0;
        public int NrSeededCities { get; set; } = 0;
        public int NrUnseededCities { get; set; } = 0;
        public int NrSeededAttractions { get; set; } = 0;
        public int NrUnseededAttractions { get; set; } = 0;
    }
}