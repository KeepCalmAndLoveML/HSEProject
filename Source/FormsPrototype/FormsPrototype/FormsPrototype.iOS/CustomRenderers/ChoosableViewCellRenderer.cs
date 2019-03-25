using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using FormsPrototype.CustomControls;
using FormsPrototype.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(ChoosableViewCell), typeof(ChoosableViewCellRenderer))]
namespace FormsPrototype.iOS.CustomRenderers
{
	public class ChoosableViewCellRenderer : ViewCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);
			var view = item as ChoosableViewCell;

			cell.SelectedBackgroundView = new UIView()
			{
				BackgroundColor = view.SelectedItemBackgroundColor.ToUIColor(),
			};

			return cell;
		}
	}
}