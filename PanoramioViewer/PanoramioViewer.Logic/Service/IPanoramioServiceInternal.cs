using System.Threading.Tasks;
using PanoramioViewer.Logic.Entity;
using Refit;

namespace PanoramioViewer.Logic.Service
{
	public interface IPanoramioServiceInternal
	{
		[Get("/get_panoramas.php?set=public&size=medium&mapfilter=true")]
		Task<PhotoResponse> GetPhotosMetadataAsync(int from, int to, string minx, string miny, string maxx, string maxy);
	}
}