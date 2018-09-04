using System;
using System.Globalization;
using System.Windows.Input;
using Jaktloggen.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen.Cells
{
    public class DateCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(DateCell), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text), 
                typeof(string), 
                typeof(DateCell), 
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                ((DateCell)bindable).TextLabel.Text = newValue as string;
                }
            );
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Label TextLabel { get; private set; }
        public Label ValueLabel { get; private set; }
        public DatePicker DatePicker { get; private set; }

        public static readonly BindableProperty DateFieldProperty =
            BindableProperty.Create(
                nameof(DateField),
                typeof(string),
                typeof(DateCell),
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                    ((DateCell)bindable).DatePicker.SetBinding(DatePicker.DateProperty, newValue as string, BindingMode.TwoWay);
                }
            );

        public string DateField
        {
            get { return (string)GetValue(DateFieldProperty); }
            set { SetValue(DateFieldProperty, value); }
        }

        /***************************************************************************/

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(
                nameof(Date),
                typeof(DateTime),
                typeof(DateCell),
                DateTime.MinValue,
                propertyChanged: (bindable, oldValue, newValue) => {
                    var dateCell = ((DateCell)bindable);
                    dateCell.ValueLabel.Text = ((DateTime)newValue).ToString("d", new CultureInfo("nb-no"));
                    dateCell.Date = (DateTime)newValue;
                    if((DateTime)oldValue == DateTime.MinValue){
                            dateCell.IsLoaded = true;
                    }
                }
            );

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { 
                SetValue(DateProperty, value); 
                OnPropertyChanged(nameof(ValueLabel));
                Command.Execute(Date);
            }
        }

        public bool IsLoaded
        {
            get;
            set;
        }

        public DateCell()
        {
            View = CreateLayout();
        }

        private StackLayout CreateLayout()
        {
            CreateTextLabel();
            CreateValueLabel();
            CreateDatePicker();

            var viewLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 10
            };

            viewLayout.Children.Add(TextLabel);
            viewLayout.Children.Add(ValueLabel);
            viewLayout.Children.Add(DatePicker);
            viewLayout.GestureRecognizers.Add(CreateTapGestureRecognizer());

            return viewLayout;
        }

        private void CreateDatePicker()
        {
            DatePicker = new DatePicker();
            DatePicker.IsVisible = false;


        }

        private TapGestureRecognizer CreateTapGestureRecognizer()
        {
            var gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += (s, e) =>
            {
                DatePicker.Focus();
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
