using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Jaktloggen.Cells
{
    public class ExtendedTextCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(ExtendedTextCell), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text), 
                typeof(string), 
                typeof(ExtendedTextCell), 
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                ((ExtendedTextCell)bindable).TextLabel.Text = newValue as string;
                }
            );
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Label TextLabel { get; private set; }

        /***************************************************************************/

        public static readonly BindableProperty DetailProperty = BindableProperty.Create(
            nameof(Detail), 
            typeof(string), 
            typeof(ExtendedTextCell), 
            "",
            propertyChanged: (bindable, oldValue, newValue) => {
            ((ExtendedTextCell)bindable).DetailLabel.Text = newValue as string;
            });

        public string Detail
        {
            get { return (string)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }

        public Label DetailLabel { get; private set; }

        /***************************************************************************/

        public static readonly BindableProperty SpinnerActiveProperty = BindableProperty.Create(
            nameof(SpinnerActive),
            typeof(bool),
            typeof(ExtendedTextCell),
            false,
            propertyChanged: (bindable, oldValue, newValue) => {
                ((ExtendedTextCell)bindable).ActivityIndicator.IsVisible = (bool)newValue;
            });

        public bool SpinnerActive
        {
            get { return (bool)GetValue(SpinnerActiveProperty); }
            set { SetValue(SpinnerActiveProperty, value); }
        }

        public ActivityIndicator ActivityIndicator { get; private set; }


        public ExtendedTextCell()
        {
            var viewLayout = new StackLayout
            {
                Orientation=StackOrientation.Horizontal,
                Padding=10
            };

            TextLabel = new Label();
            viewLayout.Children.Add(TextLabel);

            DetailLabel = new Label{HorizontalOptions = LayoutOptions.EndAndExpand};
            viewLayout.Children.Add(DetailLabel);

            ActivityIndicator = new ActivityIndicator { 
                HorizontalOptions = LayoutOptions.End,
                IsRunning = true,
                IsVisible = false
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
