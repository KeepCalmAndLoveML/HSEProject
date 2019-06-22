using FormsPrototype.iOS.CustomRenderers;
using FormsPrototype.CustomControls;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]

namespace FormsPrototype.iOS.CustomRenderers
{
	class CustomViewCellRenderer : ViewCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);

			cell.SelectionStyle = UITableViewCellSelectionStyle.None;

			return cell;
		}
	}
}