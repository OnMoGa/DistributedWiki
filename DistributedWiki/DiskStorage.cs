using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Newtonsoft.Json;

namespace DistributedWiki {
	class DiskStorage : DataSource {


		public DirectoryInfo path { get; set; }

		public DiskStorage(DirectoryInfo path) {
			this.path=path;
		}

		public override Page getPage(string title) {

			FileInfo file = path.GetFiles()
								 .FirstOrDefault(f => f.Name == $"{title}.json");
			Page page = null;
			if (file != null) {
				string json = File.ReadAllText(file.DirectoryName);
				page = JsonConvert.DeserializeObject<Page>(json);
				return page;
			}

			page = backup.getPage(title);
			if (page != null) {
				savePage(page);
			}

			return page;
		}

		public override void savePage(Page page) {
			string fileName = $"{page.title}.json";
			string fullPath = path + fileName;
			string json = page.toJson();

			using (StreamWriter file = new StreamWriter(fullPath, false)) {
				file.WriteLine(json);
			}


		}
	}
}
