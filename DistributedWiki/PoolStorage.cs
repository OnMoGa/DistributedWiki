using System;
using System.Collections.Generic;
using System.Text;
using DistributedWiki.Messages;

namespace DistributedWiki {
	class PoolStorage : DataSource {

		public Pool pool { get; set; }

		public PoolStorage(Pool pool) {
			this.pool = pool;
		}


		public override Page getPage(PageRequestMessage pageRequest) {
			Logger.log($"Getting {pageRequest.title} page from pool");
			Page page = pool.requestPage(pageRequest);

			if (page == null) {
				Logger.log($"{pageRequest.title} page not in pool");
				page = backup?.getPage(pageRequest);
			}

			if (page != null) {
				savePage(page); 
			}

			return page;
		}

		public override void savePage(Page page) {
			pool.registerNewPage(page);
		}
	}
}
