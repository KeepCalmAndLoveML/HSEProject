using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

using FormsPrototype.CustomControls;
using FormsPrototype.UWP.CustomRenderers;
using Windows.UI.Xaml;

[assembly: ExportRenderer(typeof(ChoosableViewCell), typeof(ChoosableCellRenderer))]
namespace FormsPrototype.UWP.CustomRenderers
{
	public class ChoosableCellRenderer : ViewCellRenderer
	{
		//TODO: Create a data template in xaml
		public override Windows.UI.Xaml.DataTemplate GetTemplate(Cell cell)
		{
			
			return base.GetTemplate(cell);
		}
	}
}
