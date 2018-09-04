using System;
using System.Globalization;
using System.Windows.Input;
using Jaktloggen.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen.Cells
{
    public class TimeCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(TimeCell), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text), 
                typeof(string), 
                typeof(TimeCell), 
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                ((TimeCell)bindable).TextLabel.Text = newValue as string;
                }
            );
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Label TextLabel { get; private set; }
        public Label ValueLabel { get; private set; }
        public TimePicker TimePicker { get; private set; }

        /***************************************************************************/

        public static readonly BindableProperty TimeProperty =
            BindableProperty.Create(
                nameof(Time),
                typeof(TimeSpan),
                typeof(TimeCell),
                DateTime.Now.TimeOfDay,
                propertyChanged: (bindable, oldValue, newValue) => {
                ((TimeCell)bindable).ValueLabel.Text = ((TimeSpan)newValue).ToString("h\\:mm", new CultureInfo("nb-no"));
                }
            );

        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); OnPropertyChanged(nameof(ValueLabel)); }
        }


        public TimeCell()
        {
            View = CreateLayout();
        }

        private StackLayout CreateLayout()
        {
            CreateTextLabel();
            CreateValueLabel();
            CreateTimePicker();

            var viewLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 10
            };

            viewLayout.Children.Add(TextLabel);
            viewLayout.Children.Add(ValueLabel);
            viewLayout.Children.Add(TimePicker);
            viewLayout.GestureRecognizers.Add(CreateTapGestureRecognizer());

            return viewLayout;
        }

        private void CreateTimePicker()
        {
            TimePicker = new TimePicker();
            TimePicker.SetBinding(TimePicker.TimeProperty, "Time");;
            TimePicker.IsVisible = false;
        }

        private TapGestureRecognizer CreateTapGestureRecognizer()
        {
            var gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += (s, e) =>
            {
                TimePicker.Focus();
            };
            return gestureRecognizer;
        }

        private void CreateTextLabel()
        {
            TextLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.StartAndExpand
            };
        }

        private void CreateValueLabel()
        {
            ValueLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            };
        }
    }
}
