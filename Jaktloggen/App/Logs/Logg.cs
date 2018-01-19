using System;
using System.Collections;
using System.Collections.Generic;

namespace Jaktloggen
{
    public class Logg : BaseEntity
    {
        public int Treff { get; set; }
        public int Skudd { get; set; }
        public int Sett { get; set; }
        public DateTime Dato { get; set; } = DateTime.Now;
        public string ArtId { get; set; }
        public string JegerId { get; set; }
        public string DogId { get; set; }
        public string JaktId { get; set; }
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string ImagePath { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Weather { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public string WeaponType { get; set; } = string.Empty;
        public int Weight { get; set; }
        public int ButchWeight { get; set; }
        public int Tags { get; set; }
    }
}
