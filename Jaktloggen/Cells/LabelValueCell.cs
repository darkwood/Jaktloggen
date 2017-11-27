using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Jaktloggen.Cells
{
    public class LabelValueCell : ViewCell
    {
        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create("LabelText", typeof(string), typeof(LabelValueCell), "");
        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }
        public LabelValueCell()
        {
            //instantiate each of our views
            var image = new Image();
            StackLayout cellWrapper = new StackLayout();
            StackLayout horizontalLayout = new StackLayout();
            Label left = new Label();
            Label right = new Label();

            //set bindings
            left.SetBinding(Label.TextProperty, "LabelTextProperty");
            //right.SetBinding(Label.TextProperty, "Value");
            //image.SetBinding(Image.SourceProperty, "Image");

            //Set properties for desired design
            horizontalLayout.Orientation = StackOrientation.Horizontal;
            right.HorizontalOptions = LayoutOptions.EndAndExpand;

            //add views to the view hierarchy
            //horizontalLayout.Children.Add(image);
            horizontalLayout.Children.Add(left);
            horizontalLayout.Children.Add(right);
            cellWrapper.Children.Add(horizontalLayout);
            View = cellWrapper;
        }
    }
}
