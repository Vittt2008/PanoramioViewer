using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using PanoramioViewer.Logic.Helper;
using PanoramioViewer.Logic.Service;

namespace PanoramioViewer.App.ViewModels
{
	public class PreviewPhotoViewModel : BaseViewModel
	{
		private const string OriginalPhotoUrlFormat = "http://static.panoramio.com/photos/original/{0}.jpg";
		private const int InfoWidthColumn = 360;
		private const double ScaleFactor = 0.8;

		private readonly IPanoramioService _panoramioService;
		private BitmapImage _image;
		private bool _isDownloading;

		public PreviewPhotoViewModel(PhotoViewModel photoViewModel, IPanoramioService panoramioService)
		{
			_panoramioService = panoramioService;
			CalculateSizePreviewControl(photoViewModel.Height, photoViewModel.Width);

			Lat = photoViewModel.Lat;
			Long = photoViewModel.Long;
			OwnerId = photoViewModel.OwnerId;
			OwnerName = photoViewModel.OwnerName;
			OwnerUrl = photoViewModel.OwnerUrl;
			PhotoFileUrl = photoViewModel.PhotoFileUrl;
			PhotoId = photoViewModel.PhotoId;
			PhotoTitle = photoViewModel.PhotoTitle;
			PhotoUrl = photoViewModel.PhotoUrl;
			UploadDate = photoViewModel.UploadDate;
			Image = photoViewModel.Image;
			IsDownloading = true;
		}

		private void CalculateSizePreviewControl(int height, int width)
		{
			var appSize = ResolutionHelper.GetApplicationResolution();
			var previewWidth = appSize.Width * ScaleFactor - InfoWidth;
			var aspectRatio = previewWidth / width;
			var previewHeight = height * aspectRatio;
			var maxPreviewHeight = appSize.Height * ScaleFactor;
			if (previewHeight < maxPreviewHeight)
			{
				PreviewImageHeight = (int)previewHeight;
				PreviewImageWidth = (int)previewWidth;
			}
			else
			{
				aspectRatio = maxPreviewHeight / height;
				PreviewImageHeight = (int)maxPreviewHeight;
				PreviewImageWidth = (int)(width * aspectRatio);
			}
		}

		public int PreviewImageHeight { get; private set; }
		public int PreviewImageWidth { get; private set; }

		public double Lat { get; }
		public double Long { get; }
		public long OwnerId { get; }
		public string OwnerName { get; }
		public string OwnerUrl { get; }
		public string PhotoFileUrl { get; }
		public long PhotoId { get; }
		public string PhotoTitle { get; }
		public string PhotoUrl { get; }
		public string UploadDate { get; }
		public string OriginalPhotoFileUrl => string.Format(OriginalPhotoUrlFormat, PhotoId);
		public int InfoWidth => InfoWidthColumn;
		public BitmapImage Image
		{
			get { return _image; }
			private set
			{
				if (_image == value)
					return;

				_image = value;
				OnPropertyChanged(nameof(Image));
			}
		}

		public bool IsDownloading
		{
			get { return _isDownloading; }
			private set
			{
				//if (_isDownloading == value)
				//	return;

				_isDownloading = value;
				OnPropertyChanged(nameof(IsDownloading));
			}
		}

		public DelegateCommand OnPreviewLoaded => new DelegateCommand(async args =>
		{
			Image = await _panoramioService.GetImageFromUrlAsync(OriginalPhotoFileUrl);
			IsDownloading = false;
		});



	}
}