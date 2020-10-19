using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedWiki {
	class PoolStorage : DataSource {
		public override Page getPage(string title) {
			return backup.getPage(title);
		}

		public override void savePage(Page page) {
			throw new NotImplementedException();
		}
	}
}
