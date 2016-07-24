using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media.Imaging;
using PanoramioViewer.Logic.Service;
using PanoramioViewer.Logic.ServiceImpl;

namespace PanoramioViewer.App.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private readonly IPanoramioService _panoramioService;
		private CancellationTokenSource _tokenSource;
		private PhotoViewModelCollection _images;
		private double _lat;
		private double _long;

		public MainViewModel(IPanoramioService panoramioService)
		{
			_panoramioService = panoramioService;
			_tokenSource = new CancellationTokenSource();
		}

		public IPanoramioService PanoramioService => _panoramioService;

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

		public DelegateCommand OnMapClickCommand => new DelegateCommand(async args =>
		{
			var mapArgs = args as MapInputEventArgs;
			if (mapArgs == null)
				return;

			Lat = mapArgs.Location.Position.Latitude;
			Long = mapArgs.Location.Position.Longitude;

			Images?.CancelLoadMoreItemsOperation();
			Images = new PhotoViewModelCollection(mapArgs.Location.Position, _panoramioService);
			await Images.LoadDataAsync();
		});
	}
}