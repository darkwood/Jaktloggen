﻿using System;
using System.Windows.Input;
using ImageCircle.Forms.Plugin.Abstractions;
using Xamarin.Forms;

namespace Jaktloggen.Cells
{
    public class ExtendedTextCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty = 
            BindableProperty.Create(
                nameof(Command), 
                typeof(Command), 
                typeof(ExtendedTextCell), 
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                    ((ExtendedTextCell)bindable).ViewLayout.GestureRecognizers.Add(((ExtendedTextCell)bindable).GestureRecognizer);
                }
            );

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
            propertyChanged: (bindable, oldValue, newValue) => 
            {
                ((ExtendedTextCell)bindable).DetailLabel.IsVisible = true;
                ((ExtendedTextCell)bindable).DetailLabel.Text = newValue as string;
            });

        public string Detail
        {
            get { return (string)GetValue(DetailProperty); }
            set { 
            SetValue(DetailProperty, value); }
        }

        public Label DetailLabel { get; private set; }

        /***************************************************************************/

        public static readonly BindableProperty Text2Property = BindableProperty.Create(
            nameof(Text2),
            typeof(string),
            typeof(ExtendedTextCell),
            "",
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var text = (newValue as string);
                if (text != null && text.Length > 30)
                {
                    text = text.Substring(0, 26) + "...";
                }
            ((ExtendedTextCell)bindable).Text2Label.IsVisible = true;
            ((ExtendedTextCell)bindable).Text2Label.Text = text;
            });

        public string Text2
        {
            get { return (string)GetValue(Text2Property); }
            set
            {
                SetValue(Text2Property, value);
            }
        }

        public Label Text2Label { get; private set; }

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

        /***************************************************************************/

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(
                nameof(ImageSource),
                typeof(ImageSource),
                typeof(ExtendedTextCell),
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                    ((ExtendedTextCell)bindable).CellImage.Source = newValue as ImageSource;
                    ((ExtendedTextCell)bindable).CellImage.IsVisible = true;
                }
            );

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        /***************************************************************************/

        public static readonly BindableProperty ImageSizeProperty = BindableProperty.Create(
            nameof(ImageSize),
            typeof(int),
            typeof(ExtendedTextCell),
            50,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
            ((ExtendedTextCell)bindable).CellImage.HeightRequest = (int)newValue;
            ((ExtendedTextCell)bindable).CellImage.WidthRequest = (int)newValue;
            });

        public int ImageSize
        {
            get { return (int)GetValue(ImageSizeProperty); }
            set
            {
                SetValue(ImageSizeProperty, value);
            }
        }

        /***************************************************************************/

        public static readonly BindableProperty SelectedProperty = BindableProperty.Create(
            nameof(Selected),
            typeof(bool),
            typeof(ExtendedTextCell),
            false,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((ExtendedTextCell)bindable).SelectedImage.Source = ImageSource.FromFile((bool)newValue ? "checked.png" : "checked_off.png");
            });

        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set
            {
                SetValue(SelectedProperty, value);
            }
        }

        /***************************************************************************/

        public static readonly BindableProperty ShowCheckBoxProperty = BindableProperty.Create(
            nameof(ShowCheckBox),
            typeof(bool),
            typeof(ExtendedTextCell),
            false,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
            ((ExtendedTextCell)bindable).SelectedImage.IsVisible = (bool)newValue;
            });

        public bool ShowCheckBox
        {
            get { return (bool)GetValue(ShowCheckBoxProperty); }
            set
            {
                SetValue(ShowCheckBoxProperty, value);
            }
        }

        public CircleImage CellImage { get; private set; }
        public StackLayout ViewLayout { get; private set; }
        public TapGestureRecognizer GestureRecognizer { get; private set; }
        public Image SelectedImage { get; private set; }

        public ExtendedTextCell()
        {
            ViewLayout = new StackLayout
            {
                Orientation=StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 10,
                MinimumHeightRequest=50
            };

            CellImage = new CircleImage
            {
                BorderColor = Color.White,
                BorderThickness = 0,
                Aspect = Aspect.AspectFill,
                WidthRequest = 50,
                HeightRequest= 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsVisible = false
            };
            ViewLayout.Children.Add(CellImage);

            var sublayout = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand };
            TextLabel = new Label{ Margin = 0 };
            sublayout.Children.Add(TextLabel);

            DetailLabel = new Label { Margin = 0, IsVisible = false, FontSize = 11, TextColor = Color.FromHex("#597a59") };
            sublayout.Children.Add(DetailLabel);

            ViewLayout.Children.Add(sublayout);

            Text2Label = new Label { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.EndAndExpand };
            ViewLayout.Children.Add(Text2Label);

            ActivityIndicator = new ActivityIndicator { 
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsVisible = false
            };
            ViewLayout.Children.Add(ActivityIndicator);

            SelectedImage = new Image
            {
                IsVisible = ShowCheckBox,
                Source = ImageSource.FromFile("checked_off.png"),
                Aspect = Aspect.AspectFit,
                WidthRequest = 40,
                HeightRequest = 40,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            ViewLayout.Children.Add(SelectedImage);

            GestureRecognizer = new TapGestureRecognizer();

            GestureRecognizer.Tapped += (s, e) => {
                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(null);
                }
            };

            View = ViewLayout;
        }
    }
}
