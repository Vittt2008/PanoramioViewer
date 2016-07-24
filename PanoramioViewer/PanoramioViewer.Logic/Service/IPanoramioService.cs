using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using PanoramioViewer.Logic.Entity;

namespace PanoramioViewer.Logic.Service
{
	public interface IPanoramioService
	{
		Task<PhotoInfo> FillImageAsync(PhotoInfo photoInfo);
		Task<BitmapImage> GetImageFromUrlAsync(string url);
		IEnumerable<Task<PhotoInfo>> GetBitmapImageCollectionAsync(PhotoResponse response);
		Task<PhotoResponse> GetPhotosMetadataAsync(int from, int to, double lat, double lon);
	}
}