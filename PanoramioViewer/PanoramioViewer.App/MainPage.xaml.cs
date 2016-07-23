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
		public MainPage()
		{
			InitializeComponent();
			DataContext = new MainViewModel(new PanoramioService());
		}

		public MainViewModel ViewModel => DataContext as MainViewModel;

		private void GridView_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var photoViewModel = PhotoGridView.SelectedItem as PhotoViewModel;
			//var control = new PreviewControl { DataContext = new PreviewPhotoViewModel(photoViewModel, ViewModel.PanoramioService) };
			var control = new PreviewControl(photoViewModel, ViewModel.PanoramioService);


			StackPanel stackPanel = new StackPanel();
			stackPanel.Children.Add(control);

			Border border = new Border
			{
				Child = stackPanel,
				//Background = Resources["ApplicationPageBackgroundThemeBrush"] as SolidColorBrush,
			};

			Popup popup = new Popup
			{
				Child = border,
				IsLightDismissEnabled = true,
				IsOpen = true
			};
		}
	}
}
