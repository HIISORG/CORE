using System;

namespace org.hiis {
	public class Postgres{
		#region Convert to SQL format
		/// <summary>
		/// Convert a string into SQL string with single quote around. Will return NULL if isReturnNull is true for a empty string.
		/// </summary>
		/// <param name="phrase">Original string</param>
		/// <param name="isReturnNull">Return NULL for empty string or not</param>
		/// <returns></returns>
		public static string GetSQLString(string phrase, bool isReturnNull) {
			if (string.IsNullOrEmpty(phrase)) {
				if (isReturnNull) {
					return "NULL";
				} else {
					return "''";
				}
			} else {
				return "'" + phrase.Replace("'", "''") + "'";
			}
		}
		/// <summary>
		/// Convert a ordinary string into SQL string with single quoate around. Return '' for empty string.
		/// </summary>
		/// <param name="phrase"></param>
		/// <returns></returns>
		public static string GetSQLString(string phrase) {
			return GetSQLString(phrase, false);
		}

		public static string GetSQLGUID(string phrase) {
			if (string.IsNullOrWhiteSpace(phrase)) {
				return "NULL";
			} else {
				switch (phrase.Length) {
					case 38:
						return phrase;
					case 36:
						return "'" + phrase + "'";
					default:
						try {
							return "'" + System.Guid.Parse(phrase).ToString() + "'";
						} catch {
							return "NULL";
						}
				}
			}
		}
		/// <summary>
		/// Convert a string into SQL bit type value (0 or 1). Return NULL for unconvertable value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetSQLBoolean(string value) {
			if (string.IsNullOrEmpty(value)) {
				return "NULL";
			} else if (value.ToUpper().Equals("TRUE") || value.Equals("1")) {
				return "true";
			} else if (value.ToUpper().Equals("FALSE") || value.Equals("0")) {
				return "false";
			} else {
				return "NULL";
			}
		}
		/// <summary>
		/// Convert a boolean value into SQL bit value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetSQLBoolean(bool value) {
			return value ? "1" : "0";
		}
		/// <summary>
		/// Get a SQL format date with single quote
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string GetSQLDate(string date) {
			try {
				DateTime d = Convert.ToDateTime(date);
				return "'" + d.ToString("yyyy-MM-dd") + "'";
			} catch {
				return "NULL";
			}
		}
		/// <summary>
		/// Get a SQL format date with single quote
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string GetSQLDate(DateTime date) {
			try {
				return "'" + date.ToString("yyyy-MM-dd") + "'";
			} catch {
				return "NULL";
			}
		}
		/// <summary>
		/// Get a SQL format date and time with single quote
		/// </summary>
		/// <param name="date"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public static string GetSQLDateTime(string date, string time) {
			try {
				DateTime d = Convert.ToDateTime(date);
				DateTime t = Convert.ToDateTime(time);
				return "'" + d.ToString("yyyy-MM-dd") + " " + t.ToString("HH:mm") + "'";
			} catch {
				return "NULL";
			}
		}
		/// <summary>
		/// Get a SQL format date and time with single quote
		/// </summary>
		/// <param name="date"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public static string GetSQLDateTime(DateTime date, DateTime time) {
			try {
				return "'" + date.ToString("yyyy-MM-dd") + " " + time.ToString("HH:mm") + "'";
			} catch {
				return "NULL";
			}
		}
		public static string GetSQLDateTime(DateTime datetime) {
			try {
				return "'" + datetime.ToString("yyyy-MM-dd") + " " + datetime.ToString("HH:mm:ss") + "'";
			} catch {
				return "NULL";
			}
		}
		/// <summary>
		/// Convert a string into a readable integer format string
		/// </summary>
		/// <param name="value"></param>
		/// <returns>NULL for unformated string.</returns>
		public static string GetSQLInt(string value) {
			if (string.IsNullOrEmpty(value)) {
				return "NULL";
			} else {
				try {
					return Convert.ToInt64(value).ToString();
				} catch {
					return "NULL";
				}
			}
		}
		/// <summary>
		/// Convert a integer into a string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetSQLInt(int value) {
			return value.ToString();
		}
		public static string GetSQLGeometry(string coord) {
			return "'(" + coord + ")'";
		}
		#endregion

		#region Convert from SQL format
		/// <summary>
		/// Get a short Date
		/// </summary>
		/// <param name="date">Date string</param>
		/// <returns></returns>
		public static string GetDate(string date) {
			return GetDate(date, "dd/MM/yyyy");
		}
		/// <summary>
		/// Get a short date string according to the format
		/// </summary>
		/// <param name="date">Date string</param>
		/// <param name="format">Formating string</param>
		/// <returns></returns>
		public static string GetDate(string date, string format) {
			try {
				DateTime dt = Convert.ToDateTime(date);
				return dt.ToString(format);
			} catch {
				return string.Empty;
			}
		}
		/// <summary>
		/// Get a short date string with leading zero or not. Such as 01/05/2009
		/// </summary>
		/// <param name="date">Date string</param>
		/// <param name="withZero">With leading zero or not</param>
		/// <returns></returns>
		public static string GetDate(string date, bool withZero) {
			try {
				DateTime dt = Convert.ToDateTime(date);
				if (withZero) {
					return dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
				} else {
					return dt.ToString("dd/MM/yyyy");
				}
			} catch {
				return string.Empty;
			}
		}
		/// <summary>
		/// Convert a SQL int16 or int32 format to a string vlaue for String operation.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string GetInt(string value) {
			if (string.IsNullOrEmpty(value)) {
				return "0";
			} else if (value.ToUpper().Equals("NULL")) {
				return "0";
			} else {
				try {
					return Convert.ToInt64(value).ToString();
				} catch {
					return "0";
				}
			}
		}
		/// <summary>
		/// Get a time string
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static string GetTime(string time) {
			try {
				DateTime dt = Convert.ToDateTime(time);
				return dt.ToString("HH:mm");
			} catch {
				return string.Empty;
			}
		}
		public static bool GetBoolean(string b) {
			if (string.IsNullOrWhiteSpace(b) || b == "0" || b.ToUpper() == "FALSE") {
				return false;
			} else {
				return true;
			}
		}
		public static string GetGUID(string phrase) {
			//string tmp = System.Text.RegularExpressions.Regex.Replace(phrase, @"[\W]", "");
			//if (tmp.Length != 32) {
			//    return string.Empty;
			//} else {
			//    return tmp;
			//}
			return phrase.Replace("-", "");
		}
		public static string GetGeomtry(string coord) {
			if (string.IsNullOrWhiteSpace(coord)) {
				return string.Empty;
			} else {
				string tmp = System.Text.RegularExpressions.Regex.Replace(coord, "[^0-9,.]", string.Empty, System.Text.RegularExpressions.RegexOptions.Compiled);
				if (tmp.Contains(",")) {
					return tmp;
				} else {
					return string.Empty;
				}
			}
		}
		#endregion

		public static string GetNewGUID() {
			return "'" + System.Guid.NewGuid().ToString() + "'";
		}
		public static string GetNewKey() {
			return System.Configuration.ConfigurationManager.AppSettings["SiteID"] + ((DateTime.UtcNow.Ticks - 621355968000000000) / 10000).ToString("000000000000000");
		}
	}
}
