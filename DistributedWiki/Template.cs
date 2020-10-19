using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedWiki {
	abstract class Template {


		public abstract string generateHead(Page page);
		public abstract string generateBody(Page page);
		public abstract string generateHtml(Page page);



	}
}
