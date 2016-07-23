using System.Collections.Generic;
using Newtonsoft.Json;

namespace PanoramioViewer.Logic.Entity
{
	public class PhotoResponse
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("has_more")]
		public bool HasMore { get; set; }

		[JsonProperty("map_location")]
		public MapLocation MapLocation { get; set; }

		[JsonProperty("photos")]
		public List<PhotoInfo> Photos { get; set; }
	}
}