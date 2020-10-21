using System;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace DistributedWiki {
	class Program {
		static void Main(string[] args) {

			ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
			configMap.ExeConfigFilename = @"App.config";
			Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

			if (!config.HasFile) {
				Logger.logError("No App.config detected. Exiting.");
				Console.ReadLine();
				Environment.Exit(1);
			}
			
			DirectoryInfo localStorageDirectory = new DirectoryInfo(config.AppSettings.Settings["LocalStorageDirectory"].Value);
			Uri localUri = new UriBuilder(config.AppSettings.Settings["LocalUri"].Value).Uri;
			Logger.log($"Local Uri: {localUri.toStringWithPort()}");

			PoolMember poolSeed = null;
			try {
				string poolSeedUriString = config.AppSettings.Settings["PoolSeed"].Value;
				Uri poolSeedUri = new UriBuilder(poolSeedUriString).Uri;
				Logger.log($"Pool Seed Uri: {poolSeedUri.toStringWithPort()}");
				poolSeed = new PoolMember() {
					uri = poolSeedUri
				};
			} catch (Exception e) {
				Logger.log("No PoolSeed set, assuming role of pool seed");
			}
			

			OriginStorage originStorage = new OriginStorage();

			Pool pool = new Pool(localUri, poolSeed);
			PoolStorage poolStorage = new PoolStorage(pool) {
				backup = originStorage
			};

			DiskStorage diskStorage = new DiskStorage(localStorageDirectory) {
				backup = poolStorage
			};

			MemoryStorage memoryStorage = new MemoryStorage() {
				backup = diskStorage
			};

			

			Host host = new Host(
				localUri,
				new WikipediaTemplate(),
				memoryStorage,
				pool
			);


			host.run();



		}
	}
}
