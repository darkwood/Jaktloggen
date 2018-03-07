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
                ((DateTimeCell)bindable).DateButton.Text = ((DateTime)newValue).ToString("dd.MM", new CultureInfo("nb-no"));
                ((DateTimeCell)bindable).DatePicker.Date = ((DateTime)newValue);
                }
            );

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public Button DateButton { get; private set; }

        /***************************************************************************/

        public static readonly BindableProperty TimeProperty =
            BindableProperty.Create(
                nameof(Time),
                typeof(TimeSpan),
                typeof(DateTimeCell),
                DateTime.Now.TimeOfDay,
                propertyChanged: (bindable, oldValue, newValue) => {
                    TimeSpan t = ((TimeSpan)newValue);
                    ((DateTimeCell)bindable).TimeButton.Text = $"{t.Hours}:{t.Minutes}";
                    ((DateTimeCell)bindable).TimePicker.Time = t;
                }
            );

        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public Button TimeButton { get; private set; }

        /***************************************************************************/

        public DatePicker DatePicker { get; private set; }
        public TimePicker TimePicker { get; private set; }

        public DateTimeCell()
        {
            View = CreateLayout();
        }

        private StackLayout CreateLayout()
        {
            CreateTextLabel();
            CreateDateButton();
            CreateTimeButton();
            CreateTimePicker();
            CreateDatePicker();

            var viewLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 10
            };

            viewLayout.Children.Add(TextLabel);
            viewLayout.Children.Add(DateButton);
            viewLayout.Children.Add(TimeButton);
            viewLayout.Children.Add(DatePicker);
            viewLayout.Children.Add(TimePicker);
            viewLayout.GestureRecognizers.Add(CreateTapGestureRecognizer());

            return viewLayout;
        }

        private void CreateDatePicker()
        {
            DatePicker = new DatePicker
            {
                IsVisible = false
            };
            DatePicker.SetBinding(DatePicker.DateProperty, nameof(Date));
        }

        private TapGestureRecognizer CreateTapGestureRecognizer()
        {
            var gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += (s, e) =>
            {
                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(null);
                }
            };
            return gestureRecognizer;
        }

        private void CreateTimePicker()
        {
            TimePicker = new TimePicker
            {
                IsVisible = false,
                Format = "HH:mm"
            };
            TimePicker.SetBinding(TimePicker.TimeProperty, nameof(Time));
        }

        private void CreateTimeButton()
        {
            TimeButton = new Button()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Image = "clock.png",
            };
            TimeButton.Clicked += TimeButton_Clicked;
        }

        private void CreateDateButton()
        {
            DateButton = new Button()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Image = "calendar.png",
            };
            DateButton.Clicked += DateButton_Clicked;
        }

        private void CreateTextLabel()
        {
            TextLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.StartAndExpand
            };
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
