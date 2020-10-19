using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DistributedWiki {
	class RequestHandler {

		private Host host;
		

		public Template template => host.template;
		public DataSource dataSource => host.dataSource;


		public RequestHandler(Host host) {
			this.host=host;
		}


		public string fulfillRequest(HttpListenerRequest request) {

			Logger.log($"Incoming request for {request.RawUrl}");

			List<string> urlSegments = request.Url.Segments
											  .Where(s => s != "/")
											  .Select(s=> s.Trim('/').ToLower())
											  .ToList();

			return urlSegments.Count switch
			{
				0 => getHomePage(),
				1 => getRootResources(),
				_ => getPage(urlSegments)
			};
		}

		

		private string getHomePage() {
			return "Home Page";
		}

		private string getRootResources() {
			return "Content Missing";
		}

		private string getPage(List<string> urlSegments) {
			return urlSegments[0] switch {
				"wiki" => getWikiPage(urlSegments.Skip(1).ToList()).html,
				_ => "Resource Not Found"
			};
		}



		private Page getWikiPage(List<string> urlSegments) {
			string pageTitle = urlSegments.FirstOrDefault();
			Page page = dataSource.getPage(pageTitle);

			if (page != null) {
				return page;
			}

			return new Page() {
				html = "Wiki Page Not Found"
			};
		}



	}




}
