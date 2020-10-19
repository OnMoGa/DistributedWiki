using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedWiki {
	class WikipediaTemplate : Template {


		public override string generateHead(Page page) {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<head>");



			
			stringBuilder.Append("<head>");
			return stringBuilder.ToString();
		}

		public override string generateBody(Page page) {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<body>");
			



			stringBuilder.Append("</body>");
			return stringBuilder.ToString();
		}

		public override string generateHtml(Page page) {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<html>");

			stringBuilder.Append(generateHead(page));
			stringBuilder.Append(generateBody(page));
			

			stringBuilder.Append("</html>");
			return stringBuilder.ToString();
		}





	}
}
