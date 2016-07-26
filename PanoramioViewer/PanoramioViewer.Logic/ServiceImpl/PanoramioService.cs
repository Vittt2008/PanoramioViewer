using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using PanoramioViewer.Logic.Entity;
using PanoramioViewer.Logic.Service;
using Refit;

namespace PanoramioViewer.Logic.ServiceImpl
{
	public class PanoramioService : IPanoramioService
	{
		public const string PanaromioApiServerName = "http://www.panoramio.com/map";

		private readonly IPanoramioServiceInternal _panoramioService;

		public PanoramioService()
		{
			_panoramioService = RestService.For<IPanoramioServiceInternal>(new HttpClient(new UrlLoggerHandler())
			{
				BaseAddress = new Uri(PanaromioApiServerName)
			});
		}

		public Task<PhotoResponse> GetPhotosMetadataAsync(int from, int to, double lat, double lon)
		{
			var point = new RadiousPoint(lat, lon);
			return _panoramioService.GetPhotosMetadataAsync(from, to, point.MinX, point.MinY, point.MaxX, point.MaxY);
		}

		public IEnumerable<Task<PhotoInfo>> GetBitmapImageCollectionAsync(PhotoResponse response)
		{
			return response.Photos.Select(FillImageAsync);
		}

		public async Task<PhotoInfo> FillImageAsync(PhotoInfo photoInfo)
		{
			photoInfo.Image = await GetImageFromUrlAsync(photoInfo.PhotoFileUrl);
			return photoInfo;
		}

		public async Task<BitmapImage> GetImageFromUrlAsync(string url)
		{
			var request = WebRequest.Create(url);
			using (var response = await request.GetResponseAsync())
			using (var stream = response.GetResponseStream())
			using (var memoryStream = new MemoryStream())
			{
				await stream.CopyToAsync(memoryStream);
				memoryStream.Position = 0;
				var bitmap = new BitmapImage();
				await bitmap.SetSourceAsync(memoryStream.AsRandomAccessStream());
				return bitmap;
			}
		}
	}
}