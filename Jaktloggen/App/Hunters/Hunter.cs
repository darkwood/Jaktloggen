using System;
using System.Collections;
using System.Collections.Generic;

namespace Jaktloggen
{
    public class Hunter : BaseEntity
    {
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ImagePath { get; set; }
    }
}
