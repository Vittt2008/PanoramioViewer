using System;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

namespace PanoramioViewer.Logic.Entity
{
	public class PhotoInfo
	{
		[JsonProperty("height")]
		public int Height { get; set; }

		[JsonProperty("latitude")]
		public double Lat { get; set; }

		[JsonProperty("longitude")]
		public double Long { get; set; }

		[JsonProperty("owner_id")]
		public long OwnerId { get; set; }

		[JsonProperty("owner_name")]
		public string OwnerName { get; set; }

		[JsonProperty("owner_url")]
		public string OwnerUrl { get; set; }

		[JsonProperty("photo_file_url")]
		public string PhotoFileUrl { get; set; }

		[JsonProperty("photo_id")]
		public long PhotoId { get; set; }

		[JsonProperty("photo_title")]
		public string PhotoTitle { get; set; }

		[JsonProperty("photo_url")]
		public string PhotoUrl { get; set; }

		[JsonProperty("upload_date")]
		public string UploadDate { get; set; }

		[JsonProperty("width")]
		public int Width { get; set; }

		[JsonIgnore]
		public BitmapImage Image { get; set; }
	}
}