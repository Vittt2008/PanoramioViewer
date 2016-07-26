using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media.Animation;
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
		private const string RedPinImage = "ms-appx:///Assets/pin_red.png";
		private const string GreenSmalPinImage = "ms-appx:///Assets/pin_green_2.png";
		private readonly double _zoomLevel;

		public MainPage()
		{
			InitializeComponent();
			DataContext = new MainViewModel(new PanoramioService());
			ViewModel.PreviewPhotoDownloaded += ViewModelOnPreviewPhotoDownloaded;
			_zoomLevel = (double)Resources["ZoomLevel"];
		}

		public MainViewModel ViewModel => DataContext as MainViewModel;

		private void PreviewPopupOnLayoutUpdated(object sender, object e)
		{
			double actualHorizontalOffset = PreviewPopup.HorizontalOffset;
			double actualVerticalOffset = PreviewPopup.VerticalOffset;

			double newHorizontalOffset = (Window.Current.Bounds.Width - PreviewBorder.ActualWidth) / 2;
			double newVerticalOffset = (Window.Current.Bounds.Height - PreviewBorder.ActualHeight) / 2;

			if (Math.Abs(actualHorizontalOffset - newHorizontalOffset) > 10E-6 ||
				Math.Abs(actualVerticalOffset - newVerticalOffset) > 10E-6)
			{
				PreviewPopup.HorizontalOffset = newHorizontalOffset;
				PreviewPopup.VerticalOffset = newVerticalOffset;
			}
		}

		private void MapControlOnMapTapped(MapControl sender, MapInputEventArgs args)
		{
			//Skip Event from MapElementClick
			if (ViewModel.MapClickPoint == args.Position)
				return;

			MapControl.MapElements.Clear();
			var mapElement = new MapIcon
			{
				Location = new Geopoint(new BasicGeoposition
				{
					Latitude = args.Location.Position.Latitude,
					Longitude = args.Location.Position.Longitude
				}),
				Title = $"Lat {args.Location.Position.Latitude.ToString("##.####")}\nLong {args.Location.Position.Longitude.ToString("##.####")}",
				NormalizedAnchorPoint = new Point(0.5, 1.0),
				Image = RandomAccessStreamReference.CreateFromUri(new Uri(RedPinImage)),
				ZIndex = int.MaxValue,
			};
			MapControl.MapElements.Add(mapElement);
			if (MapControl.ZoomLevel < _zoomLevel)
			{
				(Resources["ZoomStoryboard"] as Storyboard).Begin();
				MapControl.Center = mapElement.Location;
			}
		}

		private void ViewModelOnPreviewPhotoDownloaded(object sender, GeopositionArgs args)
		{
			var mapElement = new MapIcon
			{
				Location = new Geopoint(new BasicGeoposition
				{
					Latitude = args.Latitude,
					Longitude = args.Longitude
				}),
				NormalizedAnchorPoint = new Point(0.5, 1.0),
				Image = RandomAccessStreamReference.CreateFromUri(new Uri(GreenSmalPinImage))
			};
			MapControl.MapElements.Add(mapElement);
		}
	}
}
