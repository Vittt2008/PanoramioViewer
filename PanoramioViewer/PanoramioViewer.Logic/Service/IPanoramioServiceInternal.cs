using System.Threading.Tasks;
using PanoramioViewer.Logic.Entity;
using Refit;

namespace PanoramioViewer.Logic.Service
{
	public interface IPanoramioServiceInternal
	{
		[Get("/get_panoramas.php?set=public&from=0&to=20&size=medium&mapfilter=true")]
		Task<PhotoResponse> GetPhotosMetadataAsync(string minx, string miny, string maxx, string maxy);
	}
}