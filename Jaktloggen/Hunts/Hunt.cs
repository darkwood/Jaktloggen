using System;
using System.Collections;
using System.Collections.Generic;

namespace Jaktloggen
{
    public class Hunt : BaseEntity
    {
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

        DateTime dateTo;
        public DateTime DateTo
        {
            get { return dateTo; }
            set { SetProperty(ref dateTo, value); }
        }
        public List<int> HunterIds { get; set; }
        public List<int> DogIds { get; set; }
        string latitude;
        public string Latitude
        {
            get { return latitude; }
            set { 
                SetProperty(ref latitude, value); 
                OnPropertyChanged(nameof(PositionInfo));
            }
        }
        string longitude;
        public string Longitude
        {
            get { return longitude; }
            set { 
                SetProperty(ref longitude, value); 
                OnPropertyChanged(nameof(PositionInfo));
            }
        }
        public string PositionInfo
        {
            get { 
                return string.IsNullOrEmpty(Latitude) ? "" : $"{Latitude}, {Longitude}"; 
            }
        }
        public string ImagePath { get; set; }

        string notes;
        public string Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }
    }
}
