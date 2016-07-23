using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace PanoramioViewer.Logic.Helper
{
	public static class ResolutionHelper
	{
		public static Size GetApplicationResolution()
		{
			var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
			var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
			var size = new Size(bounds.Width * scaleFactor, bounds.Height * scaleFactor);
			return size;
		}
	}
}