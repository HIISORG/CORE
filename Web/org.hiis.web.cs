using System;
using System.Net;
using System.Web;

namespace org.hiis {
	public class web {

		private static bool HasColumn(System.Data.IDataRecord dr, string columnName) {
			for (int i = 0; i < dr.FieldCount; i++) {
				if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase)) return true;
			}
			return false;
		}

		public static string GetHostName(string ip) {
			try {
				return "'" + System.Net.Dns.GetHostEntry(ip).HostName + "'";
			} catch {
				return "NULL";
			}
		}

		public static string GetIP4Address(string address) {
			string IP4Address = String.Empty;

			foreach (IPAddress IPA in Dns.GetHostAddresses(address)) {
				if (IPA.AddressFamily.ToString() == "InterNetwork") {
					IP4Address = IPA.ToString();
					break;
				}
			}

			if (IP4Address != String.Empty) {
				return IP4Address;
			}

			foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName())) {
				if (IPA.AddressFamily.ToString() == "InterNetwork") {
					IP4Address = IPA.ToString();
					break;
				}
			}

			return IP4Address;
		}

		public static string GetIP4Address(HttpContext context) {
			return GetIP4Address(context.Request.UserHostAddress);
		}

		public static string GetIP4Address() {
			return GetIP4Address(HttpContext.Current.Request.UserHostAddress);
		}

	}
}
