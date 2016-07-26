using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using PanoramioViewer.Logic.Entity;
using PanoramioViewer.Logic.Service;

namespace PanoramioViewer.App.ViewModels
{
	public class PhotoViewModelCollection : ObservableCollection<PhotoViewModel>, ISupportIncrementalLoading
	{
		private const int ImagePerRequest = 20;
		private readonly IPanoramioService _panoramioService;
		private readonly BasicGeoposition _geoposition;
		private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
		private bool _isCancel;
		private int _from;
		private string _imageCount = "In Progress...";

		public event EventHandler<GeopositionArgs> PreviewPhotoDownloaded;

		public bool IsLoading { get; private set; }
		public bool HasMoreItems { get; private set; }
		public string ImageCount
		{
			get { return _imageCount; }
			private set
			{
				if (_imageCount == value)
					return;

				_imageCount = value;
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(ImageCount)));
			}
		}

		public PhotoViewModelCollection(BasicGeoposition geoposition, IPanoramioService panoramioService)
		{
			_geoposition = geoposition;
			_panoramioService = panoramioService;
			HasMoreItems = false;
		}

		public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
		{
			return AsyncInfo.Run(c => LoadDataAsync());
		}


		public void CancelLoadMoreItemsOperation()
		{
			_isCancel = true;
			_tokenSource.Cancel();
		}

		public async Task<LoadMoreItemsResult> LoadDataAsync()
		{
			IsLoading = true;
			HasMoreItems = false;

			var data = await _panoramioService.GetPhotosMetadataAsync(_from, _from + ImagePerRequest, _geoposition.Latitude, _geoposition.Longitude);
			var tasks = _panoramioService.GetBitmapImageCollectionAsync(data).ToList();
			var count = tasks.Count;
			ImageCount = CalculateImageCount(data, count);

			if (count == 0)
				return TerminateDownloadingWithResult();

			while (tasks.Count > 0)
			{
				if (_isCancel)
					return TerminateDownloadingWithResult();

				var photo = await WaitFirstBitmapRequestAsync(tasks);

				if (IsLoading)
					RemoveLoadingViewModel();

				Add(new PhotoViewModel(photo));
				OnPreviewPhotoDownloaded(new GeopositionArgs(photo.Lat, photo.Long));
			}

			if (data.HasMore)
				Add(PhotoViewModel.CreateLoadingViewModel());

			IsLoading = data.HasMore;
			HasMoreItems = data.HasMore;
			_from = _from + ImagePerRequest;

			return new LoadMoreItemsResult { Count = (uint)count };
		}

		private LoadMoreItemsResult TerminateDownloadingWithResult()
		{
			IsLoading = false;
			HasMoreItems = false;
			return new LoadMoreItemsResult();
		}

		private static async Task<PhotoInfo> WaitFirstBitmapRequestAsync(List<Task<PhotoInfo>> tasks)
		{
			var task = await Task.WhenAny(tasks);
			tasks.Remove(task);
			var photo = await task;
			return photo;
		}

		private void RemoveLoadingViewModel()
		{
			IsLoading = false;
			var item = this.FirstOrDefault(x => x.IsLoading);
			if (item != null)
				Remove(item);
		}

		private string CalculateImageCount(PhotoResponse data, int imageTaskCount)
		{
			if (data.HasMore)
				return data.Count.ToString();
			return imageTaskCount.ToString();
		}

		protected virtual void OnPreviewPhotoDownloaded(GeopositionArgs e)
		{
			PreviewPhotoDownloaded?.Invoke(this, e);
		}
	}
}