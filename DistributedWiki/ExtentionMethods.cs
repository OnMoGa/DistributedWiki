using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedWiki {
	static class ExtentionMethods {

		static Random random = new Random();

		public static string toStringWithPort(this Uri uri) {
			return $"{uri.Host}:{uri.Port}";
		}




		public static T getRandomElement<T>(this IEnumerable<T> collection) {
			int length = collection.Count();
			if (length == 0) {
				return default(T);
			}
			int index = random.Next(length)-1;
			return collection.Skip(index).First();
		}





	}
}
