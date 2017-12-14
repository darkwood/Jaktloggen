using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Jaktloggen.Cells
{
    public class ImageHeaderCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(ImageHeaderCell), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(
                nameof(Source), 
                typeof(ImageSource), 
                typeof(ImageHeaderCell), 
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                    ((ImageHeaderCell)bindable).CellImage.Source = newValue as ImageSource;
                }
            );
        
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly BindableProperty HeightRequestProperty = BindableProperty.Create(
            nameof(HeightRequest), 
            typeof(string), 
            typeof(ImageHeaderCell), 
            "150",
            propertyChanged: (bindable, oldValue, newValue) => {
                ((ImageHeaderCell)bindable).CellImage.HeightRequest = double.Parse(newValue as string);
            });

        public string HeightRequest
        {
            get { return (string)GetValue(HeightRequestProperty); }
            set { SetValue(HeightRequestProperty, value); }
        }

        public Image CellImage { get; private set; }


        public ImageHeaderCell()
        {
            var viewLayout = new StackLayout();

            CellImage = new Image();
            CellImage.Aspect = Aspect.AspectFill;
            viewLayout.Children.Add(CellImage);

            var gestureRecognizer = new TapGestureRecognizer();

            gestureRecognizer.Tapped += (s, e) => {
                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(null);
                }
            };

            //You can either set the binding here or you can set the binding in the xaml. You only need one or the other.
            //Xaml for this is below
            //SetBinding(CommandProperty, new Binding("ItemSelectedCommand"));

            viewLayout.GestureRecognizers.Add(gestureRecognizer);

            View = viewLayout;
        }
    }
}
