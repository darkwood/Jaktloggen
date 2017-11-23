using System;
using System.Collections;
using System.Collections.Generic;

namespace Jaktloggen
{
    public class Hunt : BaseViewModel
    {
        public string Id { get; set; }
        public string Location { get; set; }

        DateTime dateFrom;
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }

        public DateTime DateTo { get; set; }
        public List<int> HunterIds { get; set; }
        public List<int> DogIds { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ImagePath { get; set; }
        public string Notes { get; set; }
    }
}
