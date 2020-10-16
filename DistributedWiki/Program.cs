using System;

namespace DistributedWiki {
	class Program {
		static void Main(string[] args) {

			Host host = new Host();

			host.pages.Add(new Page() {
				text = "Test Page 1"

			});

			host.run();



		}
	}
}
