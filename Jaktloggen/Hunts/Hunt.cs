using System;
using System.Collections;
using System.Collections.Generic;

namespace Jaktloggen
{
    public class Hunt
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<int> HunterIds { get; set; }
        public List<int> DogIds { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ImagePath { get; set; }
        public string Notes { get; set; }
    }
}
