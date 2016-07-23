using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PanoramioViewer.Logic.ServiceImpl
{
	public class UrlLoggerHandler : HttpClientHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var url = request.RequestUri;
			Debug.WriteLine(url);
			return base.SendAsync(request, cancellationToken);
		}
	}
}