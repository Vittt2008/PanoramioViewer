using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;

namespace PanoramioViewer.Logic.Helper
{
	public static class BitmapHelper
	{
		public static async Task SavePhotoFromUrl(this StorageFile storageFile, string url)
		{
			using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
			using (var imageStream = await RandomAccessStreamReference.CreateFromUri(new Uri(url)).OpenReadAsync())
			{
				await RandomAccessStream.CopyAsync(imageStream, stream);
			}
		}
	}
}