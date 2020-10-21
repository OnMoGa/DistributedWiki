using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedWiki.Messages {
	class PageRequestMessage {

		public string title { get; set; }

		public List<string> ignorantPeerUris { get; set; } = new List<string>();
	}
}
