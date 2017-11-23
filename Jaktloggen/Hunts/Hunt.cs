using System;
using System.Collections;
using System.Collections.Generic;

namespace Jaktloggen
{
    public class Hunt : BaseViewModel
    {
        public string Id { get; set; }

        string location;
        public string Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }

        DateTime dateFrom;
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { SetProperty(ref dateFrom, value); }
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
