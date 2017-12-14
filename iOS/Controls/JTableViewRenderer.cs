using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Jaktloggen.JTableView), typeof(Jaktloggen.iOS.Controls.JTableViewRenderer))]
namespace Jaktloggen.iOS.Controls
{
    public class JTableViewRenderer : TableViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                return;

            var tableView = Control as UITableView;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (JTableView)Element;
            Control.Source = new JUnEvenTableViewModelRenderer(view);
        }

        public class JUnEvenTableViewModelRenderer : UnEvenTableViewModelRenderer
        {
            public JUnEvenTableViewModelRenderer(TableView model) : base(model)
            {
            }

            public override nfloat GetHeightForHeader(UITableView tableView, nint section)
            {
                if(section == 0)
                {
                    return 0.0f;
                }
                return base.GetHeightForHeader(tableView, section);
            }

            public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                var cell = base.GetCell(tableView, indexPath);
                cell.ContentView.BackgroundColor = UIColor.White;
                return cell;
            }
        }
    }
}