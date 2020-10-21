using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using DistributedWiki.Messages;
using Newtonsoft.Json;

namespace DistributedWiki {
	class RequestHandler {

		private Host host;
		

		public Template template => host.template;
		public DataSource dataSource => host.dataSource;

		public Pool pool => host.pool;

		public RequestHandler(Host host) {
			this.host=host;
		}


		public string handleRequest(HttpListenerRequest request) {

			Logger.log($"Incoming request for {request.RawUrl}");

			List<string> urlSegments = request.Url.Segments
											  .Where(s => s != "/")
											  .Select(s=> s.Trim('/').ToLower())
											  .ToList();

			return urlSegments.Count switch
			{
				0 => getHomePage(),
				1 => getRootResources(),
				_ => handleSubRequest(urlSegments, request)
			};
		}

		

		private string getHomePage() {
			return "Home Page";
		}

		private string getRootResources() {
			return "Content Missing";
		}

		private string handleSubRequest(List<string> urlSegments, HttpListenerRequest request) {
			return urlSegments.First() switch {
				"wiki" => getWikiPage(urlSegments.Skip(1).ToList(), request).text,
				"data" => getWikiPage(urlSegments.Skip(1).ToList(), request).toJson(),
				"pool" => pool == null ? "Server is not connected to pool" : processPoolRequest(urlSegments.Skip(1).ToList(), request),
				_ => "Resource Not Found"
			};
		}


		private Page getWikiPage(List<string> urlSegments, HttpListenerRequest request) {
			string pageTitle = urlSegments.FirstOrDefault();



			Page page = dataSource.getPage(new PageRequestMessage{title = pageTitle});

			if (page != null) {
				return page;
			}

			return new Page() {
				html = "Wiki Page Not Found"
			};
		}


		private string processPoolRequest(List<string> urlSegments, HttpListenerRequest request) {

			if (urlSegments.First() == "register") {
				using (var reader = new StreamReader(request.InputStream, request.ContentEncoding)) {
					string json = reader.ReadToEnd();
					PoolMember newMember = JsonConvert.DeserializeObject<PoolMember>(json);

					pool.registerNewUser(newMember);
					return "{'success': true}";

				}
			}
			
			if (urlSegments.First() == "peerlist") {
				return JsonConvert.SerializeObject(pool.members);
			}
			
			return "";
		}








	}




}
