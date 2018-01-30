using System;
using System.Globalization;
using System.Windows.Input;
using Jaktloggen.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen.Cells
{
    public class DateTimeCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(DateTimeCell), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text), 
                typeof(string), 
                typeof(DateTimeCell), 
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                ((DateTimeCell)bindable).TextLabel.Text = newValue as string;
                }
            );
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Label TextLabel { get; private set; }

        /***************************************************************************/

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(
                nameof(Date),
                typeof(DateTime),
                typeof(DateTimeCell),
                DateTime.Now,
                propertyChanged: (bindable, oldValue, newValue) => {
                ((DateTimeCell)bindable).DateButton.Text = ((DateTime)newValue).ToString("d", new CultureInfo("nb-no"));
                ((DateTimeCell)bindable).TimeButton.Text = ((DateTime)newValue).ToString("t", new CultureInfo("nb-no"));
                ((DateTimeCell)bindable).DatePicker.Date = ((DateTime)newValue);
                ((DateTimeCell)bindable).TimePicker.Time = ((DateTime)newValue).TimeOfDay;
                }
            );

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public Button DateButton { get; private set; }
        public Button TimeButton { get; private set; }

        /***************************************************************************/

        public DatePicker DatePicker { get; private set; }
        public TimePicker TimePicker { get; private set; }

        public DateTimeCell()
        {
            var viewLayout = new StackLayout
            {
                Orientation=StackOrientation.Horizontal,
                Padding = 10,
            };

            TextLabel = new Label{
                VerticalOptions = LayoutOptions.Center
            };
            viewLayout.Children.Add(TextLabel);


            DateButton = new Button()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Image = "Statistics.png",
                Margin = 5
            };
            //viewLayout.Children.Add(DateButton);

            TimeButton = new Button()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Image = "Felter.png",
                Margin = 5
            };
            viewLayout.Children.Add(TimeButton);

            DateButton.Clicked += DateButton_Clicked;
            TimeButton.Clicked += TimeButton_Clicked;

            DatePicker = new DatePicker
            {
                //IsVisible = false
            };
            DatePicker.SetBinding(DatePicker.DateProperty, nameof(Date));

            viewLayout.Children.Add(DatePicker);

            TimePicker = new TimePicker
            {
                IsVisible = false
            };
           // TimePicker.SetBinding(TimePicker.TimeProperty, nameof(Date));

            viewLayout.Children.Add(TimePicker);

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

        void DateButton_Clicked(object sender, EventArgs e)
        {
            DatePicker.Focus();
        }
        void TimeButton_Clicked(object sender, EventArgs e)
        {
            TimePicker.Focus();
        }
    }
}
