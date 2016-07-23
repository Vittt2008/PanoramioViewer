using System;
using System.Globalization;

namespace PanoramioViewer.Logic.Entity
{
	public class RadiousPoint
	{
		public const double LatOffset = 0.25;
		public const double LonOffset = 0.5;

		private static readonly IFormatProvider Provider = new NumberFormatInfo { CurrencyDecimalSeparator = "." };

		public RadiousPoint(double lat, double lon)
		{
			MinX = (lon - LonOffset).ToString(Provider);
			MinY = (lat - LatOffset).ToString(Provider);
			MaxX = (lon + LonOffset).ToString(Provider);
			MaxY = (lat + LatOffset).ToString(Provider);
		}

		public string MinX { get; }
		public string MinY { get; }
		public string MaxX { get; }
		public string MaxY { get; }
	}
}