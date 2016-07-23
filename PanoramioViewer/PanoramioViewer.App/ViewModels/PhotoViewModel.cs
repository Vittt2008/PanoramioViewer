using Windows.UI.Xaml.Media.Imaging;
using PanoramioViewer.Logic.Entity;

namespace PanoramioViewer.App.ViewModels
{
	public class PhotoViewModel : BaseViewModel
	{
		public PhotoViewModel(PhotoInfo photoInfo)
		{
			Height = photoInfo.Height;
			Lat = photoInfo.Lat;
			Long = photoInfo.Long;
			OwnerId = photoInfo.OwnerId;
			OwnerName = photoInfo.OwnerName;
			OwnerUrl = photoInfo.OwnerUrl;
			PhotoFileUrl = photoInfo.PhotoFileUrl;
			PhotoId = photoInfo.PhotoId;
			PhotoTitle = photoInfo.PhotoTitle;
			PhotoUrl = photoInfo.PhotoUrl;
			UploadDate = photoInfo.UploadDate;
			Width = photoInfo.Width;
			Image = photoInfo.Image;
		}

		public int Height { get; }
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
		public int Width { get; }
		public BitmapImage Image { get; }
		public bool IsLoading => Image == null && string.IsNullOrEmpty(PhotoFileUrl);
	}
}