using System;
using System.Collections.Generic;
using System.Text;
using DistributedWiki.Messages;

namespace DistributedWiki {
	abstract class DataSource {


		public DataSource backup { get; set; }


		public abstract Page getPage(PageRequestMessage pageRequest);
		public abstract void savePage(Page page);

	}
}
