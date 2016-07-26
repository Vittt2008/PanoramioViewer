using System;

namespace PanoramioViewer.App.ViewModels
{
	public class GeopositionArgs : EventArgs
	{
		public GeopositionArgs(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		public double Latitude { get; }
		public double Longitude { get; }
	}
}