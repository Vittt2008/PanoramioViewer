using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media.Imaging;
using PanoramioViewer.Logic.Service;
using PanoramioViewer.Logic.ServiceImpl;

namespace PanoramioViewer.App.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private readonly IPanoramioService _panoramioService;
		private PhotoViewModelCollection _images;
		private double _lat;
		private double _long;
		private PreviewPhotoViewModel _previewPhoto;
		private bool _isPreviewOpen;

		public MainViewModel(IPanoramioService panoramioService)
		{
			_panoramioService = panoramioService;
		}

		public IPanoramioService PanoramioService => _panoramioService;

		public event EventHandler<GeopositionArgs> PreviewPhotoDownloaded;

		public PhotoViewModelCollection Images
		{
			get { return _images; }
			private set
			{
				if (_images == value)
					return;

				_images = value;
				OnPropertyChanged(nameof(Images));
			}
		}

		public double Lat
		{
			get { return _lat; }
			private set
			{
				if (_lat == value)
					return;

				_lat = value;
				OnPropertyChanged(nameof(Lat));
			}
		}

		public double Long
		{
			get { return _long; }
			private set
			{
				if (_long == value)
					return;

				_long = value;
				OnPropertyChanged(nameof(Long));
			}
		}

		public PreviewPhotoViewModel PreviewPhoto
		{
			get { return _previewPhoto; }
			set
			{
				if (_previewPhoto == value)
					return;

				_previewPhoto = value;
				OnPropertyChanged(nameof(PreviewPhoto));
			}
		}

		public bool IsPreviewOpen
		{
			get { return _isPreviewOpen; }
			set
			{
				if (_isPreviewOpen == value)
					return;

				_isPreviewOpen = value;
				OnPropertyChanged(nameof(IsPreviewOpen));
			}
		}

		public Point MapClickPoint { get; private set; }

		public DelegateCommand OnMapClickCommand => new DelegateCommand(async args =>
		{
			var mapArgs = args as MapInputEventArgs;
			if (mapArgs == null)
				return;

			if (MapClickPoint == mapArgs.Position)
				return;

			Lat = mapArgs.Location.Position.Latitude;
			Long = mapArgs.Location.Position.Longitude;

			if (Images != null)
			{
				Images.CancelLoadMoreItemsOperation();
				Images.PreviewPhotoDownloaded -= ImagesOnPreviewPhotoDownloaded;
			}

			Images = new PhotoViewModelCollection(mapArgs.Location.Position, _panoramioService);
			Images.PreviewPhotoDownloaded += ImagesOnPreviewPhotoDownloaded;

			await Images.LoadDataAsync();
		});

		public DelegateCommand OnMapElementClick => new DelegateCommand(args =>
		{
			var mapArgs = args as MapElementClickEventArgs;

			var mapElement = mapArgs?.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
			if (mapElement == null)
				return;

			MapClickPoint = mapArgs.Position;
			var photoViewModels = Images.Where(x => Math.Abs(x.Lat - mapElement.Location.Position.Latitude) < 10E-6 &&
													Math.Abs(x.Long - mapElement.Location.Position.Longitude) < 10E-6).ToList();
			var photoViewModel = photoViewModels.FirstOrDefault();
			if (photoViewModel == null)
				return;

			PreviewPhoto = new PreviewPhotoViewModel(photoViewModel, PanoramioService);
			IsPreviewOpen = true;
		});

		public DelegateCommand OnItemClick => new DelegateCommand(args =>
		{
			var eventArgs = args as ItemClickEventArgs;
			var photoViewModel = eventArgs?.ClickedItem as PhotoViewModel;
			if (photoViewModel == null)
				return;
			PreviewPhoto = new PreviewPhotoViewModel(photoViewModel, PanoramioService);
			IsPreviewOpen = true;
		});

		private void ImagesOnPreviewPhotoDownloaded(object sender, GeopositionArgs args)
		{
			OnPreviewPhotoDownloaded(args);
		}

		protected virtual void OnPreviewPhotoDownloaded(GeopositionArgs e)
		{
			PreviewPhotoDownloaded?.Invoke(this, e);
		}
	}
}