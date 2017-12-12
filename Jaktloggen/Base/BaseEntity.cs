using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class BaseEntity : BaseViewModel
    {
        public virtual string ID { get; set; }
        public DateTime Created { get; set; } = DateTime.MinValue;
        public DateTime Changed { get; set; } = DateTime.Now;
        public string UserId { get; set; }
    }
}
