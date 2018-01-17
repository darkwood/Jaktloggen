using System;
using System.Windows.Input;
using Jaktloggen.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen.Cells
{
    public class MapCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(MapCell), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text), 
                typeof(string), 
                typeof(MapCell), 
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                ((MapCell)bindable).TextLabel.Text = newValue as string;
                }
            );
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Label TextLabel { get; private set; }
        public Label PositionLabel { get; private set; }

        /***************************************************************************/

        public static readonly BindableProperty LatitudeProperty = BindableProperty.Create(
            nameof(Latitude), 
            typeof(double), 
            typeof(MapCell), 
            (double)0,
            propertyChanged: (bindable, oldValue, newValue) => 
            {
                ((MapCell)bindable).PositionLabel.Text = ((double)newValue).ToString();
            });

        public double Latitude
        {
            get { return (double)GetValue(LatitudeProperty); }
            set { 
                SetValue(LatitudeProperty, value);
            }
        }

        /***************************************************************************/


        public static readonly BindableProperty LongitudeProperty = BindableProperty.Create(
            nameof(Longitude),
            typeof(double),
            typeof(MapCell),
            (double)0,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((MapCell)bindable).PositionLabel.Text += ", " + ((double)newValue).ToString();
                ((MapCell)bindable).SetMapPosition();
            });

        public double Longitude
        {
            get { return (double)GetValue(LongitudeProperty); }
            set
            {
                SetValue(LongitudeProperty, value);
            }
        }


        public ActivityIndicator ActivityIndicator { get; private set; }
        public ExtendedMap MyMap { get; private set; }
        public Position Position => new Position(Latitude, Longitude);

        private void SetMapPosition()
        {
            ActivityIndicator.IsVisible = false;
            MyMap.IsVisible = true;

            MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    Position, Distance.FromMeters(50)));

            var pin = new Pin()
            {
                Position = Position,
                Label = "Valgt posisjon"
            };
            MyMap.Pins.Clear();
            MyMap.Pins.Add(pin);
        }

        public MapCell()
        {
            var viewLayout = new StackLayout
            {
                Orientation=StackOrientation.Horizontal,
                Padding = 10,
                HeightRequest = 100
            };

            TextLabel = new Label{
                VerticalOptions = LayoutOptions.Center
            };
            viewLayout.Children.Add(TextLabel);

            PositionLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center
            };
            //viewLayout.Children.Add(PositionLabel);

            MyMap = new ExtendedMap
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                MapType = Xamarin.Forms.Maps.MapType.Hybrid,
                HeightRequest = 100,
                WidthRequest = 180,
                IsEnabled = false,
                IsVisible = false
            };
            viewLayout.Children.Add(MyMap);

            ActivityIndicator = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsVisible = true
            };
            viewLayout.Children.Add(ActivityIndicator);

            var gestureRecognizer = new TapGestureRecognizer();

            gestureRecognizer.Tapped += (s, e) => {
                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(null);
                }
            };

            viewLayout.GestureRecognizers.Add(gestureRecognizer);

            View = viewLayout;
        }
    }
}
