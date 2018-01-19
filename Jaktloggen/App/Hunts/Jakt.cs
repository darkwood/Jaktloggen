using System;
using System.Collections;
using System.Collections.Generic;

namespace Jaktloggen
{
    public class Jakt : BaseEntity
    {
        public string Sted { get; set; }
        public DateTime DatoFra { get; set; }
        public DateTime DatoTil { get; set; }
        public List<string> JegerIds { get; set; }
        public List<string> DogIds { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ImagePath { get; set; }
        public string Notes { get; set; }
    }
}
