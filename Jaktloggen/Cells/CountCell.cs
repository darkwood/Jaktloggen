using System;
using System.Windows.Input;
using ImageCircle.Forms.Plugin.Abstractions;
using Xamarin.Forms;

namespace Jaktloggen.Cells
{
    public class CountCell : ViewCell
    {
        private Color green => Color.FromHex("#597a59");

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(CountCell), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /***************************************************************************/

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text), 
                typeof(string), 
                typeof(CountCell), 
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                ((CountCell)bindable).TextLabel.Text = newValue as string;
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
            typeof(CountCell), 
            "",
            propertyChanged: (bindable, oldValue, newValue) => 
            {
                ((CountCell)bindable).DetailLabel.IsVisible = true;
                ((CountCell)bindable).DetailLabel.Text = newValue as string;
            });

        public string Detail
        {
            get { return (string)GetValue(DetailProperty); }
            set { 
            SetValue(DetailProperty, value); }
        }

        public Label DetailLabel { get; private set; }

        /***************************************************************************/

        public static readonly BindableProperty CountProperty = BindableProperty.Create(
            nameof(Count),
            typeof(int),
            typeof(CountCell),
            0,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((CountCell)bindable).SetSelectedButton((int)newValue);
            });



        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set
            {
                SetValue(CountProperty, value);
            }
        }

        public StackLayout ViewLayout { get; private set; }
        public TapGestureRecognizer GestureRecognizer { get; private set; }
        public Image SelectedImage { get; private set; }
        private StackLayout _buttons { get; set; }

        public CountCell()
        {
            ViewLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 10,
            };

            var sublayout = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand };
            TextLabel = new Label { Margin = 0 };
            sublayout.Children.Add(TextLabel);

            DetailLabel = new Label { Margin = 0, IsVisible = false, FontSize = 11, TextColor = Color.FromHex("#597a59") };
            sublayout.Children.Add(DetailLabel);

            ViewLayout.Children.Add(sublayout);

            _buttons = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.EndAndExpand };
            CreateButtons();
            ViewLayout.Children.Add(_buttons);

            View = ViewLayout;
        }


        private void CreateButtons()
        {
            _buttons.Children.Clear();
            _buttons.Children.Add(CreateRoundButton(0));
            _buttons.Children.Add(CreateRoundButton(1));
            _buttons.Children.Add(CreateRoundButton(2));
            _buttons.Children.Add(CreateRoundButton(3));
            Button moreButton = CreateRoundButton(Count, true);
            moreButton.BorderColor = green;
            moreButton.BackgroundColor = Color.White;
            moreButton.TextColor = green;
            moreButton.BorderRadius = 0;
            _buttons.Children.Add(moreButton);
        }

        private Button CreateRoundButton(int i, bool isMoreButton = false)
        {
            
            var b = new Button
            {
                Text = i.ToString(),
                BorderRadius = 23,
                HeightRequest = 46,
                WidthRequest = 46,
                TextColor = i == Count ? Color.White : green,
                BackgroundColor = i == Count ? green : Color.White,
                BorderColor = green,
                BorderWidth = 2
            };

                b.Clicked += (sender, e) => {
                    Count = i;
                    if (Command != null && Command.CanExecute(null))
                    {
                        Command.Execute(isMoreButton);
                    }

                };
            return b;
        }
        private void SetSelectedButton(int selectedValue)
        {
            CreateButtons();
        }
    }
}
