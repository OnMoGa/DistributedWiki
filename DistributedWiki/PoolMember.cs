using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata;
using System.Text;

namespace DistributedWiki {
	class PoolMember {

		public Uri uri { get; set; }
		public List<string> pageTitles { get; set; } = new List<string>();

		public override string ToString() {
			return uri.toStringWithPort();
		}

		public override bool Equals(object obj) {
			if (!(obj is PoolMember)) return false;
			PoolMember comparison = (PoolMember)obj;
			return comparison.uri.toStringWithPort() == uri.toStringWithPort();
		}
	}
}
