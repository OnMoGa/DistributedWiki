using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DistributedWiki {
	class Host {
		private HttpListener httpListener;


		public List<Page> pages { get; set; } = new List<Page>();


		public Host() {
			httpListener = new HttpListener();
			httpListener.Prefixes.Add("http://*:80/");
			
		}



		public void run() {
			httpListener.Start();
			Console.WriteLine("Listening...");

			while (true) {
				HttpListenerContext context = httpListener.GetContext();
				HttpListenerRequest request = context.Request;
				// Obtain a response object.
				HttpListenerResponse response = context.Response;
				// Construct a response.
				string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
				byte[] buffer = Encoding.UTF8.GetBytes(responseString);
				// Get a response stream and write the response to it.
				response.ContentLength64 = buffer.Length;
				System.IO.Stream output = response.OutputStream;
				output.Write(buffer,0,buffer.Length);
				// You must close the output stream.
				output.Close();
			}




		}




	}
}
