using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedWiki {
	class Page {

		public int id { get; set; }
		public string title { get; set; }
		public string path { get; set; }
		public DateTime timestamp { get; set; }
		public string text { get; set; }

		[JsonIgnore]
		public string sha1 { get; set; }

		[JsonIgnore]
		public string html { get; set; }




		public string toJson() {
			return JsonConvert.SerializeObject(this);
		}
		
	}
}
