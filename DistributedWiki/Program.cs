using System;
using System.IO;

namespace DistributedWiki {
	class Program {
		static void Main(string[] args) {


			DataSource dataSource = new MemoryStorage() {
				backup = new DiskStorage(new DirectoryInfo(@"C:\Users\Michael\Desktop\wikipages")) {
					backup = new PoolStorage(new UriBuilder("192.168.0.100").Uri) {
						backup = new OriginStorage()
					}
				}
			};



			Host host = new Host(
				new WikipediaTemplate(),
				dataSource
			);

			host.run();



		}
	}
}
