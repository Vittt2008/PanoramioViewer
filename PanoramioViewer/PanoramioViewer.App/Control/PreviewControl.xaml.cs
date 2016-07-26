﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PanoramioViewer.App.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PanoramioViewer.App.Control
{
	public sealed partial class PreviewControl : UserControl
	{
		private const int ZoomLevel = 10;

		public static DependencyProperty ViewModelProperty =
			DependencyProperty.Register(
				"ViewModel",
				typeof(PreviewPhotoViewModel),
				typeof(PreviewControl),
				new PropertyMetadata(null, PropertyChangedCallback));

		private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			var previewControl = dependencyObject as PreviewControl;
			var viewModel = args.NewValue as PreviewPhotoViewModel;
			previewControl.DataContext = viewModel;
			previewControl.MapControl.MapElements.Clear();
			previewControl.MapControl.MapElements.Add(viewModel.MapElement);
			previewControl.MapControl.ZoomLevel = ZoomLevel;
			previewControl.MapControl.Center = viewModel.MapElement.Location;
			viewModel.OnPreviewLoaded.Execute(null);
		}

		public PreviewControl()
		{
			InitializeComponent();
		}

		public PreviewPhotoViewModel ViewModel
		{
			get { return GetValue(ViewModelProperty) as PreviewPhotoViewModel; }
			set { SetValue(ViewModelProperty, value); }
		}

		/*public PreviewPhotoViewModel ViewModel
		{
			get { return DataContext as PreviewPhotoViewModel; }
			set
			{
				DataContext = value;
				MapControl.MapElements.Clear();
				MapControl.MapElements.Add(ViewModel.MapElement);
				MapControl.ZoomLevel = ZoomLevel;
				MapControl.Center = ViewModel.MapElement.Location;
			}
		}*/
	}
}
