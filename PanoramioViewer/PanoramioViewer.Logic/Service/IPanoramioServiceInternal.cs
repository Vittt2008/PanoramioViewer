using System.Threading.Tasks;
using PanoramioViewer.Logic.Entity;
using Refit;

namespace PanoramioViewer.Logic.Service
{
	public interface IPanoramioServiceInternal
	{
		[Get("/get_panoramas.php?set=public&from=0&to=20&minx=-180&miny=-90&maxx=180&maxy=90&size=medium&mapfilter=true")]
		Task<PhotoResponse> GetPhotosMetadataAsync();
	}
}