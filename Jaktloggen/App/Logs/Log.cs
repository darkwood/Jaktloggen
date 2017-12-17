using System;
using System.Collections;
using System.Collections.Generic;

namespace Jaktloggen
{
    public class Log : BaseEntity
    {
        public string HuntId { get; set; }
        public DateTime Date { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ImagePath { get; set; }
        public string Notes { get; set; }
    }
}
