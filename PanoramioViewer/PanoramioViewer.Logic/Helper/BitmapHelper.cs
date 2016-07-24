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
		public static Task<WriteableBitmap> ToWriteableBitmap(this BitmapImage bitmapImage)
		{
			return WriteableBitmapFromBitmapImageExtension.LoadFromBitmapImageSourceAsync(bitmapImage);
		}

		public static async Task<RandomAccessStreamReference> ToRandomAccessStream(this BitmapImage bitmapImage)
		{
			using (var stream = new InMemoryRandomAccessStream())
			{
				var writeableBitmap = new WriteableBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
				await writeableBitmap.LoadFromBitmapImageSourceAsync(bitmapImage);
				await writeableBitmap.ToStreamAsJpeg(stream);
				var streamReference = RandomAccessStreamReference.CreateFromStream(stream);
				return streamReference;
			}
		}

		public static async Task<IRandomAccessStream> ToIRandomAccessStream(this BitmapImage bitmapImage)
		{
			var stream = new InMemoryRandomAccessStream();
			var writeableBitmap = new WriteableBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
			await writeableBitmap.LoadFromBitmapImageSourceAsync(bitmapImage);
			await writeableBitmap.ToStreamAsJpeg(stream);
			return stream;
		}

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