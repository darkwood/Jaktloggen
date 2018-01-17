using System;
using System.Collections;
using System.Collections.Generic;

namespace Jaktloggen
{
    public class Dog : BaseEntity
    {
        public string Navn { get; set; }
        public string Registreringsnummer { get; set; }
        public string Rase { get; set; }
        public string ImagePath { get; set; }
    }
}
