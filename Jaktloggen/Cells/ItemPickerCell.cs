using System;
using System.Collections.Generic;
using System.Windows.Input;
using ImageCircle.Forms.Plugin.Abstractions;
using Xamarin.Forms;
using System.Linq;

using Jaktloggen.Models;
using System.Threading.Tasks;

namespace Jaktloggen.Cells
{
    public class ItemPickerCell : ViewCell
    {
        private Color green => Color.FromHex("#597a59");

        private const int SIZE = 52;
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(ItemPickerCell), null);

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
                typeof(ItemPickerCell), 
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                ((ItemPickerCell)bindable).TextLabel.Text = newValue as string;
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
            typeof(ItemPickerCell), 
            "",
            propertyChanged: (bindable, oldValue, newValue) => 
            {
                ((ItemPickerCell)bindable).DetailLabel.IsVisible = true;
                ((ItemPickerCell)bindable).DetailLabel.Text = newValue as string;
            });

        public string Detail
        {
            get { return (string)GetValue(DetailProperty); }
            set { 
            SetValue(DetailProperty, value); }
        }

        public Label DetailLabel { get; private set; }

        /***************************************************************************/

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            nameof(SelectedItem),
            typeof(PickerItem),
            typeof(ItemPickerCell),
            null,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((ItemPickerCell)bindable).SelectItem((PickerItem)newValue);
            });


        public PickerItem SelectedItem
        {
            get { return (PickerItem)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        /***************************************************************************/

        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
            nameof(Items),
            typeof(List<PickerItem>),
            typeof(ItemPickerCell),
            null,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((ItemPickerCell)bindable).CreateButtons();
            });



        public List<PickerItem> Items
        {
            get { return (List<PickerItem>)GetValue(ItemsProperty); }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        public StackLayout ViewLayout { get; private set; }
        public TapGestureRecognizer GestureRecognizer { get; private set; }
        public Image SelectedImage { get; private set; }
        private StackLayout _buttonsLayout { get; set; }
        public List<CircleImage> _circleImages { get; private set; }

        public ItemPickerCell()
        {
            ViewLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 10,
                HeightRequest = SIZE + 15,
                WidthRequest = SIZE
            };

            var sublayout = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand };
            TextLabel = new Label { Margin = 0 };
            sublayout.Children.Add(TextLabel);

            DetailLabel = new Label { Margin = 0, IsVisible = false, FontSize = 11, TextColor = Color.FromHex("#597a59") };
            sublayout.Children.Add(DetailLabel);

            ViewLayout.Children.Add(sublayout);

            _buttonsLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.EndAndExpand };
            ViewLayout.Children.Add(_buttonsLayout);

            View = ViewLayout;
        }


        private void CreateButtons()
        {
            _buttonsLayout.Children.Clear();
            _circleImages = new List<CircleImage>();
            foreach(var item in Items)
            {
                var layout = new StackLayout();
                layout.GestureRecognizers.Add(CreateTapGestureRecognizer(item, layout));

                var img = CreateCircleImage(item);
                SetSelectionStyle(img, item.Selected);
                _circleImages.Add(img);
                layout.Children.Add(img);

                layout.Children.Add(new Label
                {
                    Text = GetTitle(item),
                    FontSize = 10,
                    HorizontalOptions = LayoutOptions.Center,
                });
                _buttonsLayout.Children.Add(layout);
            }
            _buttonsLayout.Children.Add(CreateMoreButton());
        }

        private static string GetTitle(PickerItem item)
        {
            return item.Title.Length >= 10 ? item.Title.Substring(0, 7) + "..." : item.Title;
        }

        private CircleImage CreateCircleImage(PickerItem i)
        {
            return new CircleImage
            {
                Source = i.ImageSource,
                HeightRequest = SIZE,
                WidthRequest = SIZE,
                BorderThickness = 4,
                Aspect = Aspect.AspectFill,
                StyleId = i.ID
            };
        }

        private StackLayout CreateMoreButton()
        {
            var layout = new StackLayout();
            layout.GestureRecognizers.Add(CreateTapGestureRecognizer(null, layout));

            layout.Children.Add(new Frame
            {
                CornerRadius = 0,
                HeightRequest = SIZE,
                WidthRequest = SIZE,
				BorderColor = green,
                HasShadow = false,
                Padding = 0,
                Content = new Label
                {
                    Text = "...",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = green,
                }
            });
            return layout;
        }

        private void SelectItem(PickerItem item)
        {
            foreach (var img in _circleImages)
            {
                SetSelectionStyle(img, item?.ID == img.StyleId);
            }
        }

        private void SetSelectionStyle(CircleImage b, bool selected)
        {
            b.BackgroundColor = selected ? green : Color.Transparent;
            b.BorderColor = selected ? Color.DarkOrange : green;
            b.Opacity = selected ? 1 : 0.5;
        }

        private TapGestureRecognizer CreateTapGestureRecognizer(PickerItem i, View btnImage)
        {
            var gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += async (s, e) =>
            {
                await btnImage.ScaleTo(0.75, 50, Easing.Linear);
                await btnImage.ScaleTo(1, 50, Easing.Linear);

                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(i);
                }
            };
            return gestureRecognizer;
        }
    }
}
