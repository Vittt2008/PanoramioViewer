using System.Collections.ObjectModel;
using System.Linq;
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

		public MainViewModel(IPanoramioService panoramioService)
		{
			_panoramioService = panoramioService;
			Images = new ObservableCollection<PhotoViewModel>();
		}

		public ObservableCollection<PhotoViewModel> Images { get; }
		public IPanoramioService PanoramioService => _panoramioService;

		public DelegateCommand OnMapClickCommand => new DelegateCommand(async args =>
		{
			var mapArgs = args as MapInputEventArgs;
			if (mapArgs == null)
				return;

			var data = await _panoramioService.GetPhotosMetadataAsync(mapArgs.Location.Position.Latitude, mapArgs.Location.Position.Longitude);

			Images.Clear();

			var tasks = _panoramioService.GetBitmapImageCollectionAsync(data).ToList();
			while (tasks.Count > 0)
			{
				var task = await Task.WhenAny(tasks);
				tasks.Remove(task);
				var photo = await task;
				Images.Add(new PhotoViewModel(photo));
			}
		});
	}
}