using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using PanoramioViewer.Logic.Entity;

namespace PanoramioViewer.Logic.Service
{
	public interface IPanoramioService : IPanoramioServiceInternal
	{
		Task<PhotoInfo> FillImageAsync(PhotoInfo photoInfo);
		Task<BitmapImage> GetImageFromUrlAsync(string url);
		IEnumerable<Task<PhotoInfo>> GetBitmapImageCollectionAsync(PhotoResponse response);
	}
}