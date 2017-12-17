//using Xamarin.Forms;

//namespace Jaktloggen.Cells
//{
//    public enum TextCellDetailLocation
//    {
//        Right,
//        Below
//    }

//    public class JTextCell : TextCell
//    {
//        public JTextCell()
//        {
            
//        }

//        public static readonly BindableProperty DetailLocationProperty = BindableProperty.Create<JTextCell, TextCellDetailLocation>(p => p.DetailLocation, default(TextCellDetailLocation));

//        public TextCellDetailLocation DetailLocation
//        {
//            get { return (TextCellDetailLocation)GetValue(DetailLocationProperty); }
//            set { SetValue(DetailLocationProperty, value); }
//        }

//        public static readonly BindableProperty FontSizeProperty =
//            BindableProperty.Create<JTextCell, double>(
//                p => p.FontSize, -1);
        
//        public double FontSize
//        {
//            get { return (double)GetValue(FontSizeProperty); }
//            set { SetValue(FontSizeProperty, value); }
//        }

//        /// <summary>
//        /// The background color property
//        /// </summary>
//        public static readonly BindableProperty BackgroundColorProperty =
//            BindableProperty.Create<JTextCell, Color>(p => p.BackgroundColor, Color.Transparent);

//        /// <summary>
//        /// Gets or sets the color of the background.
//        /// </summary>
//        /// <value>The color of the background.</value>
//        public Color BackgroundColor
//        {
//            get { return (Color)GetValue(BackgroundColorProperty); }
//            set { SetValue(BackgroundColorProperty, value); }
//        }

//        /// <summary>
//        /// The separator color property
//        /// </summary>
//        public static readonly BindableProperty SeparatorColorProperty =
//            BindableProperty.Create<JTextCell, Color>(p => p.SeparatorColor, Color.FromRgba(199, 199, 204, 255));

//        /// <summary>
//        /// Gets or sets the color of the separator.
//        /// </summary>
//        /// <value>The color of the separator.</value>
//        public Color SeparatorColor
//        {
//            get { return (Color)GetValue(SeparatorColorProperty); }
//            set { SetValue(SeparatorColorProperty, value); }
//        }

//        /// <summary>
//        /// The separator padding property
//        /// </summary>
//        public static readonly BindableProperty SeparatorPaddingProperty =
//            BindableProperty.Create<JTextCell, Thickness>(p => p.SeparatorPadding, default(Thickness));

//        /// <summary>
//        /// Gets or sets the separator padding.
//        /// </summary>
//        /// <value>The separator padding.</value>
//        public Thickness SeparatorPadding
//        {
//            get { return (Thickness)GetValue(SeparatorPaddingProperty); }
//            set { SetValue(SeparatorPaddingProperty, value); }
//        }


//        /// <summary>
//        /// The show separator property
//        /// </summary>
//        public static readonly BindableProperty ShowSeparatorProperty =
//            BindableProperty.Create<JTextCell, bool>(p => p.ShowSeparator, true);

//        /// <summary>
//        /// Gets or sets a value indicating whether [show separator].
//        /// </summary>
//        /// <value><c>true</c> if [show separator]; otherwise, <c>false</c>.</value>
//        public bool ShowSeparator
//        {
//            get { return (bool)GetValue(ShowSeparatorProperty); }
//            set { SetValue(ShowSeparatorProperty, value); }
//        }


//        /// <summary>
//        /// The show disclousure property
//        /// </summary>
//        public static readonly BindableProperty ShowDisclousureProperty =
//            BindableProperty.Create<JTextCell, bool>(p => p.ShowDisclousure, false);

//        /// <summary>
//        /// Gets or sets a value indicating whether [show disclousure].
//        /// </summary>
//        /// <value><c>true</c> if [show disclousure]; otherwise, <c>false</c>.</value>
//        public bool ShowDisclousure
//        {
//            get { return (bool)GetValue(ShowDisclousureProperty); }
//            set { SetValue(ShowDisclousureProperty, value); }
//        }

//        /// <summary>
//        /// The disclousure image property
//        /// </summary>
//        public static readonly BindableProperty DisclousureImageProperty =
//            BindableProperty.Create<JTextCell, string>(
//                p => p.DisclousureImage, string.Empty);

//        /// <summary>
//        /// Gets or sets the disclousure image.
//        /// </summary>
//        /// <value>The disclousure image.</value>
//        public string DisclousureImage
//        {
//            get { return (string)GetValue(DisclousureImageProperty); }
//            set { SetValue(DisclousureImageProperty, value); }
//        }
//    }
//}
