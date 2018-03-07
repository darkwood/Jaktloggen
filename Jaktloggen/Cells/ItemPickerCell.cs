using System;
using System.Collections.Generic;
using System.Windows.Input;
using ImageCircle.Forms.Plugin.Abstractions;
using Xamarin.Forms;
using System.Linq;

using Jaktloggen.Models;

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
        public List<Button> _buttons { get; private set; }

        public ItemPickerCell()
        {
            ViewLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 10,
                HeightRequest = SIZE
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
            _buttons = Items.Select(i => CreateRoundButton(i)).ToList();
            foreach(var b in _buttons){
                _buttonsLayout.Children.Add(b);
            }
            _buttonsLayout.Children.Add(CreateRoundButton(null));
        }

        private Button CreateRoundButton(PickerItem i)
        {

            var b = new Button
            {
                Text = i == null ? "..." : i.Title,
                FontSize = 10,
                BorderRadius = SIZE/2,
                HeightRequest = SIZE,
                WidthRequest = SIZE,
                BorderWidth = 2
            };

            SetSelectionStyle(b, i?.Selected ?? false);

            if (i == null)
            {
                b.BorderRadius = 0;
            }
            b.Clicked += (sender, e) =>
            {
                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(i);
                }

            };
            return b;
        }

        private void SelectItem(PickerItem item)
        {
            foreach (var btn in _buttons)
            {
                SetSelectionStyle(btn, btn.Text == item.Title);
            }
        }

        private void SetSelectionStyle(Button b, bool selected)
        {
            b.TextColor = selected ? Color.White : green;
            b.BackgroundColor = selected ? green : Color.White;
            b.BorderColor = selected ? Color.DarkOrange : green;
        }
    }
}
