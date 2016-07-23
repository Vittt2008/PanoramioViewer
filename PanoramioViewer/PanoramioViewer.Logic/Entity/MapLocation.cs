using Newtonsoft.Json;

namespace PanoramioViewer.Logic.Entity
{
	public class MapLocation
	{
		[JsonProperty("lat")]
		public double Lat { get; set; }

		[JsonProperty("lon")]
		public double Long { get; set; }

		[JsonProperty("panoramio_zoom")]
		public int Zoom { get; set; }
	}
}