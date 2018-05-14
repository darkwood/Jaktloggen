using System;
using System.Collections;
using System.Collections.Generic;

namespace Jaktloggen
{
    public class LoggType : BaseEntity
    {
        public string Key { get; set; }
        public string Navn { get; set; }
        public string Beskrivelse { get; set; }
        public string GroupId { get; set; }
    }
}
