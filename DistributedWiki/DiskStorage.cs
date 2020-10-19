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
			if (!path.Exists) {
				path.Create();
			}
			this.path=path;
		}

		public override Page getPage(string title) {

			FileInfo file = path.GetFiles().FirstOrDefault(f => f.Name.Equals($"{title}.json", StringComparison.InvariantCultureIgnoreCase));
			Page page;
			if (file != null) {
				string fileName = $"{title.ToLower()}.json";
				string json = File.ReadAllText(Path.Combine(path.FullName, fileName));
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
			string fullPath = Path.Combine(path.FullName, fileName);
			string json = page.toJson();

			using (StreamWriter file = new StreamWriter(fullPath, false)) {
				file.WriteLine(json);
			}


		}
	}
}
