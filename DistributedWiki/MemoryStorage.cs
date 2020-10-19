using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedWiki {
	class MemoryStorage : DataSource {


		public List<Page> pages { get; set; } = new List<Page>();

		public override Page getPage(string title) {
			Page page = pages.FirstOrDefault(p => p.title == title);

			if(page == null) {
				page = backup.getPage(title);

				if (page != null) {
					savePage(page);
				}
			}

			return page;
		}

		public override void savePage(Page page) {
			pages.Add(page);
		}
	}
}
