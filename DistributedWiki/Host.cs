using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection.Metadata;
using System.Text;

namespace DistributedWiki {
	class Host {
		private HttpListener httpListener;


		public RequestHandler requestHandler { get; set; }

		public Template template { get; set; }
		public DataSource dataSource { get; set; }
		public Pool pool { get; set; }
		public readonly Uri localUri;

		public Host(Uri localUri, Template template, DataSource dataSource) {
			httpListener = new HttpListener();
			httpListener.Prefixes.Add($"http://*:{localUri.Port}/");

			requestHandler = new RequestHandler(this);
			this.dataSource = dataSource;
			this.template = template;
			this.localUri = localUri;
		}



		public void run() {
			try {
				httpListener.Start();
			} catch (HttpListenerException e) {
				Logger.logError($"Use of port {localUri.Port} is denied");
				Console.ReadLine();
				Environment.Exit(1);
			}
			Console.WriteLine("Listening...");

			while (true) {
				HttpListenerContext context = httpListener.GetContext();
				HttpListenerRequest request = context.Request;
				HttpListenerResponse response = context.Response;

				string responseString = requestHandler.handleRequest(request);
				byte[] buffer = Encoding.UTF8.GetBytes(responseString);

				// Get a response stream and write the response to it.
				response.ContentLength64 = buffer.Length;

				using (Stream output = response.OutputStream) {
					try {
						output.Write(buffer,0,buffer.Length);
					} catch (HttpListenerException e) {

					}
					
				}
			}

		}




	}
}
