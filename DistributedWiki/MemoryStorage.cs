using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedWiki.Messages;

namespace DistributedWiki {
	class MemoryStorage : DataSource {


		public List<Page> pages { get; set; } = new List<Page>();

		public override Page getPage(PageRequestMessage pageRequest) {
			Logger.log($"Getting {pageRequest.title} page from memory");
			Page page = pages.FirstOrDefault(p => p.title.Equals(pageRequest.title, StringComparison.InvariantCultureIgnoreCase));

			if(page == null) {
				Logger.log($"{pageRequest.title} page not in memory");
				page = backup?.getPage(pageRequest);

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
