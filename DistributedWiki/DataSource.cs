using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedWiki {
	abstract class DataSource {


		public DataSource backup { get; set; }


		public abstract Page getPage(string title);
		public abstract void savePage(Page page);

	}
}
