using System;
using Jaktloggen.iOS.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(ExtendedEntryRenderer))]
namespace Jaktloggen.iOS.Controls
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);
            var nativeTextField = (UITextField)Control;
            nativeTextField.EditingDidBegin += (object sender, EventArgs eIos) => {
                //nativeTextField.PerformSelector(new Selector("selectAll"), null, 0.0f);
                nativeTextField.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
            };
        }
    }
}
