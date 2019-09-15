using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Input;
using Xamarin.Forms;

namespace RussianModnik.CustomControls
{
	public class ClickableListView : ListView
	{
		public static BindableProperty ItemClickCommandProperty = BindableProperty.Create(nameof(ItemClickCommand), typeof(ICommand), typeof(ClickableListView), null);

		public ICommand ItemClickCommand
		{
			get => (ICommand)this.GetValue(ItemClickCommandProperty);
			set => this.SetValue(ItemClickCommandProperty, value);
		}

		public ClickableListView()
		{
			this.ItemTapped += ChoosableListView_ItemTapped;
			this.ItemSelected += ClickableListView_ItemSelected;
		}

		private void ClickableListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			object item;
			if (this.TryGetSelectedItem(e, out item))
			{
				ItemClickCommand?.Execute(item);
			}
		}

		private void ChoosableListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (e.Item != null)
			{
				ItemClickCommand?.Execute(e.Item);
				SelectedItem = null;
			}
		}
	}
}
