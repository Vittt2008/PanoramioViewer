using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PanoramioViewer.App.Control;
using PanoramioViewer.App.ViewModels;
using PanoramioViewer.Logic.ServiceImpl;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PanoramioViewer.App
{
	/// <summary>
	/// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private readonly PreviewControl _previewControl;
		private readonly Popup _popup;

		public MainPage()
		{
			InitializeComponent();
			_previewControl = new PreviewControl();
			_popup = CreatePopup(_previewControl);
			DataContext = new MainViewModel(new PanoramioService());
		}

		public MainViewModel ViewModel => DataContext as MainViewModel;

		private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var photoViewModel = PhotoGridView.SelectedItem as PhotoViewModel;
			if (photoViewModel == null || photoViewModel.IsLoading)
				return;

			_previewControl.ViewModel = new PreviewPhotoViewModel(photoViewModel, ViewModel.PanoramioService);
			_popup.IsOpen = true;
		}

		private Popup CreatePopup(PreviewControl previewControl)
		{
			var stackPanel = new StackPanel { Children = { previewControl } };
			var border = new Border
			{
				Child = stackPanel,
				BorderThickness = new Thickness(1),
				BorderBrush = Resources["ApplicationForegroundThemeBrush"] as SolidColorBrush
			};
			var popup = new Popup
			{
				Child = border,
				IsLightDismissEnabled = true,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};

			popup.LayoutUpdated += (o, args) =>
			{
				double actualHorizontalOffset = popup.HorizontalOffset;
				double actualVerticalOffset = popup.VerticalOffset;

				double newHorizontalOffset = (Window.Current.Bounds.Width - border.ActualWidth) / 2;
				double newVerticalOffset = (Window.Current.Bounds.Height - border.ActualHeight) / 2;

				if (Math.Abs(actualHorizontalOffset - newHorizontalOffset) > 10E-6 ||
					Math.Abs(actualVerticalOffset - newVerticalOffset) > 10E-6)
				{
					popup.HorizontalOffset = newHorizontalOffset;
					popup.VerticalOffset = newVerticalOffset;
				}
			};

			return popup;
		}
	}
}
