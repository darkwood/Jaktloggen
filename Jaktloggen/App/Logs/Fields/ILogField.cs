using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jaktloggen
{
    public interface ILogField
    {
        string FieldLabel { get; set; }
        ICommand TapCommand { get; set; }

    }
}
