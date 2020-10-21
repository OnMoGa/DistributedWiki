using DistributedWiki.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace DistributedWiki {
	class Pool {

		public PoolMember memberInfo { get; set; }
		public List<PoolMember> members { get; set; } = new List<PoolMember>();

		private static readonly HttpClient client = new HttpClient();

		public Pool(Uri localUri, PoolMember poolSeed) {
			memberInfo = new PoolMember() {
				uri = localUri
			};

			if(poolSeed != null) {
				registerWithPool(poolSeed);
			}
		}


		private void registerWithPool(PoolMember peerListSource) {
			HttpResponseMessage peerListResponse;

			try {
				peerListResponse = client.GetAsync($"{peerListSource.uri.toStringWithPort()}pool/peerlist").Result;
			} catch(Exception e) {
				if (e is HttpRequestException || e is AggregateException) {
					Logger.logError($"Failed to connect to pool. Assuming role of seed.");
					return;
				}
				throw;
			}

			string peerListJson = peerListResponse.Content.ReadAsStringAsync().Result;
				
			members = JsonConvert.DeserializeObject<List<PoolMember>>(peerListJson);
			registerNewUser(peerListSource);

			Logger.log($"{members.Count} members in pool");
			foreach(PoolMember poolMember in members) {
				Logger.log($"{poolMember}");

				StringContent form = new StringContent(JsonConvert.SerializeObject(memberInfo));

				HttpResponseMessage registerResponse = client.PostAsync($"{peerListSource.uri.toStringWithPort()}pool/register", form).Result;
			}
		}


		public bool registerNewUser(PoolMember newMember) {
			if(members.Contains(newMember)) {
				Logger.log($"{newMember} reconnected");
			} else {
				Logger.log($"{newMember} connected");
				members.Add(newMember);
			}
			return true;
		}


		public void registerNewPage(Page page) {
			memberInfo.pageTitles.Add(page.title);
		}


		public Page requestPage(PageRequestMessage pageRequest) {
			pageRequest.ignorantPeerUris.Add(memberInfo.uri.toStringWithPort());

			PoolMember nonIgnorantPoolMember = members.FirstOrDefault(m => !pageRequest.ignorantPeerUris.Contains(m.uri.toStringWithPort()));

			Page page = null;
			if(nonIgnorantPoolMember != null) {
				StringContent form = new StringContent(JsonConvert.SerializeObject(pageRequest.ignorantPeerUris));
				Logger.log($"Asking {nonIgnorantPoolMember.uri.toStringWithPort()} for the {pageRequest.title} page");
				HttpResponseMessage pageResponse = client.PostAsync($"{nonIgnorantPoolMember.uri.toStringWithPort()}data/{pageRequest.title}", form).Result;

				string responseString = pageResponse.Content.ReadAsStringAsync().Result;
				page = JsonConvert.DeserializeObject<Page>(responseString);
			}

			return page;
		}
	}
}
