using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedWiki {
	class Pool {

		public Uri tracker { get; set; }

		public List<PoolMember> members { get; set; } = new List<PoolMember>();



	}
}
