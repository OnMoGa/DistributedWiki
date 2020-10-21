using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedWiki {
	static class ExtentionMethods {



		public static string toStringWithPort(this Uri uri) {
			return uri.ToString().Insert(uri.ToString().Length - 1, $":{uri.Port}");
		}



	}
}
