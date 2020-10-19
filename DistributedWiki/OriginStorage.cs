using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace DistributedWiki {
	class OriginStorage : DataSource {


		private static readonly HttpClient client = new HttpClient();

		public override Page getPage(string title) {
			

			Dictionary<string, string> values = new Dictionary<string, string>
			{
				{ "pages", title },
				{ "curonly", "1" }
			};

			FormUrlEncodedContent content = new FormUrlEncodedContent(values);

			HttpResponseMessage response = client.PostAsync("https://en.wikipedia.org/wiki/Special:Export", content).Result;

			string responseString = response.Content.ReadAsStringAsync().Result;

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(responseString);

			XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
			nsManager.AddNamespace("wiki", "http://www.mediawiki.org/xml/export-0.10/");

			XmlNode pageXML = doc.DocumentElement.SelectSingleNode("wiki:page", nsManager);
			string pageTitle = pageXML.SelectSingleNode("wiki:title", nsManager).InnerText;
			int id = int.Parse(pageXML.SelectSingleNode("wiki:id", nsManager).InnerText);


			XmlNode revision = pageXML.SelectSingleNode("wiki:revision", nsManager);
			string text = revision.SelectSingleNode("wiki:text", nsManager).InnerText;
			string sha1 = revision.SelectSingleNode("wiki:sha1", nsManager).InnerText;
			DateTime timestamp = DateTime.Parse(revision.SelectSingleNode("wiki:timestamp", nsManager).InnerText);


			return new Page() {
				id = id,
				title = pageTitle,
				text = text,
				timestamp = timestamp,
				path = $"wiki/{pageTitle}",
				sha1 = sha1
			};
		}

		public override void savePage(Page page) {
			//Nothing to do
		}
	}
}
