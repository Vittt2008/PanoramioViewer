using System;
using System.Collections.Generic;
using System.Xml;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media.Imaging;
using NotificationsExtensions;
using NotificationsExtensions.Toasts;
using PanoramioViewer.Logic.Helper;
using PanoramioViewer.Logic.Service;

namespace PanoramioViewer.App.ViewModels
{
	public class PreviewPhotoViewModel : BaseViewModel
	{
		private const string OriginalPhotoUrlFormat = "http://static.panoramio.com/photos/original/{0}.jpg";
		private const string GreenPinImage = "ms-appx:///Assets/pin_green.png";
		private const int InfoWidthColumn = 360;
		private const double ScaleFactor = 0.8;

		private readonly IPanoramioService _panoramioService;
		private BitmapImage _image;
		private bool _isDownloading;
		private int _previewImageHeight;
		private int _previewImageWidth;

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

		public int PreviewImageHeight
		{
			get { return _previewImageHeight; }
			private set
			{
				if (_previewImageHeight == value)
					return;

				_previewImageHeight = value;
				OnPropertyChanged(nameof(PreviewImageHeight));
			}
		}

		public int PreviewImageWidth
		{
			get { return _previewImageWidth; }
			private set
			{
				if (_previewImageWidth == value)
					return;

				_previewImageWidth = value;
				OnPropertyChanged(nameof(PreviewImageWidth));
			}
		}

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
				if (_isDownloading == value)
					return;

				_isDownloading = value;
				OnPropertyChanged(nameof(IsDownloading));
			}
		}

		public MapIcon MapElement
		{
			get
			{
				return new MapIcon
				{
					Location = new Geopoint(new BasicGeoposition
					{
						Latitude = Lat,
						Longitude = Long
					}),
					Image = RandomAccessStreamReference.CreateFromUri(new Uri(GreenPinImage)),
					Title = $"Lat {Lat.ToString("##.####")}\nLong {Long.ToString("##.####")}",
					NormalizedAnchorPoint = new Point(0.5, 1.0)
				};
			}
		}

		public DelegateCommand OnPreviewLoaded => new DelegateCommand(async args =>
		{
			Image = await _panoramioService.GetImageFromUrlAsync(OriginalPhotoFileUrl);
			IsDownloading = false;
		});

		public DelegateCommand OnShareCommand => new DelegateCommand(args =>
		{
			DataTransferManager.ShowShareUI();
			DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;
		});

		public DelegateCommand OnSaveCommand => new DelegateCommand(async args =>
		{
			StorageFile storageFile = null;
			try
			{
				var saverPiker = CreateJpegFileSavePicker();
				storageFile = await saverPiker.PickSaveFileAsync();
				if (storageFile == null)
					return;
				await storageFile.SavePhotoFromUrl(OriginalPhotoFileUrl);
				ShowSuccessfulToast(storageFile);
			}
			catch (Exception ex)
			{
				ShowErrorToast(storageFile, ex.Message);
			}
		});

		private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
		{
			args.Request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(OriginalPhotoFileUrl)));
			args.Request.Data.SetText(PhotoTitle);
			args.Request.Data.Properties.Title = Windows.ApplicationModel.Package.Current.DisplayName;
		}

		private FileSavePicker CreateJpegFileSavePicker()
		{
			var saverPiker = new FileSavePicker();
			saverPiker.FileTypeChoices.Add(".jpg Image", new List<string> { ".jpg" });
			saverPiker.DefaultFileExtension = ".jpg";
			saverPiker.SuggestedFileName = $"{PhotoTitle}.jpg";
			return saverPiker;
		}

		private void ShowSuccessfulToast(StorageFile file)
		{
			ToastContent content = new ToastContent()
			{
				Visual = new ToastVisual
				{
					BindingGeneric = new ToastBindingGeneric
					{
						AppLogoOverride = new ToastGenericAppLogo
						{
							Source = file.Path,
							HintCrop = ToastGenericAppLogoCrop.Default,
						},
						Children =
						{
							new AdaptiveText { Text = file.Name },
							new AdaptiveText { Text = "File was saved to disk." },
							new AdaptiveText { Text = file.Path },
						},
					},
				},
				Audio = new ToastAudio()
				{
					Src = new Uri("ms-winsoundevent:Notification.IM")
				},
			};

			ShowToast(content);
		}

		private void ShowErrorToast(StorageFile file, string message)
		{
			ToastContent content = new ToastContent()
			{
				Visual = new ToastVisual
				{
					BindingGeneric = new ToastBindingGeneric
					{
						Children =
						{
							new AdaptiveText { Text = "An error occurred while saving the file." },
							new AdaptiveText { Text = message },
							new AdaptiveText { Text = file?.Path },
						},
					},
				},
				Audio = new ToastAudio()
				{
					Src = new Uri("ms-winsoundevent:Notification.IM")
				},
			};

			ShowToast(content);
		}

		private static void ShowToast(ToastContent content)
		{
			var doc = content.GetXml();
			var toast = new ToastNotification(doc);
			ToastNotificationManager.CreateToastNotifier().Show(toast);
		}
	}
}