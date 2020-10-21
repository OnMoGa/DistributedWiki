using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using DistributedWiki.Messages;
using Newtonsoft.Json;

namespace DistributedWiki {
	class DiskStorage : DataSource {


		public DirectoryInfo path { get; set; }

		public DiskStorage(DirectoryInfo path) {
			if (!path.Exists) {
				path.Create();
			}
			this.path=path;
		}

		public override Page getPage(PageRequestMessage pageRequest) {
			Logger.log($"Getting {pageRequest.title} page from disk");
			FileInfo file = path.GetFiles().FirstOrDefault(f => f.Name.Equals($"{pageRequest.title}.json", StringComparison.InvariantCultureIgnoreCase));
			Page page = null;

			if (file != null) {
				string fileName = $"{pageRequest.title.ToLower()}.json";
				string json = File.ReadAllText(Path.Combine(path.FullName, fileName));
				page = JsonConvert.DeserializeObject<Page>(json);
			}

			if (file == null) {
				Logger.log($"{pageRequest.title} page not on disk");
				page = backup?.getPage(pageRequest);
				if (page != null) {
					savePage(page);
				}
			}

			return page;
		}

		public override void savePage(Page page) {
			string fileName = $"{page.title}.json";
			string fullPath = Path.Combine(path.FullName, fileName);
			string json = page.toJson();

			using (StreamWriter file = new StreamWriter(fullPath, false)) {
				file.WriteLine(json);
			}


		}
	}
}
