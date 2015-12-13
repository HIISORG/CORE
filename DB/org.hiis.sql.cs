using System;

namespace org.hiis {
	public static class SQL {
		#region Convert to SQL format
		public static string GetSQLInt(string input) {
			if (string.IsNullOrWhiteSpace(input)) {
				return "NULL";
			} else {
				try {
					return Convert.ToInt64(input).ToString();
				} catch {
					return "NULL";
				}
			}
		}
		public static string GetSQLInt(int input) {
			return input.ToString();
		}
		#endregion
	}
}
