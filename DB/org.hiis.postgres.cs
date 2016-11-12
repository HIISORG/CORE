using System;
using System.Web;

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
				return GetNewGUID();
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
			return value ? "true" : "false";
		}
		/// <summary>
		/// Get a SQL format date with single quote
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string GetSQLDate(string date) {
			try {
				DateTime d;
				if (System.Web.HttpContext.Current != null) d = System.DateTime.ParseExact(date, System.Web.HttpContext.Current.Session["DTFMT"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				else d = Convert.ToDateTime(date);
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
		public static string GetSQLDateTime(string date)
		{
			return GetSQLDateTime (date, "00:00");
		}
		public static string GetSQLDateTime(string date, string time) {
			try {
				DateTime d;
				if (System.Web.HttpContext.Current != null) d = System.DateTime.ParseExact(date, System.Web.HttpContext.Current.Session["DTFMT"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				else d = Convert.ToDateTime(date);
				if (string.IsNullOrWhiteSpace(time)) {
					return "'" + d.ToString("yyyy-mm-dd") + "'";
				} else {
					DateTime t = Convert.ToDateTime(time);
					return "'" + d.ToString("yyyy-MM-dd") + " " + t.ToString("HH:mm") + "'";
				}
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
		public static string GetSQLDateTime(DateTime datetime)
		{
			try {
				return "'" + datetime.ToString ("yyyy-MM-dd") + " " + datetime.ToString ("HH:mm:ss") + "'";
			} catch {
				return "NULL";
			}
		}
		public static string GetSQLDateTime(DateTime date, DateTime time) {
			try {
				return "'" + date.ToString("yyyy-MM-dd") + " " + time.ToString("HH:mm") + "'";
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
		public static string GetSQLInt(Int64 value) {
			return value.ToString();
		}
		public static string GetSQLDecimal(string value) {
			if (string.IsNullOrWhiteSpace(value)) {
				return "NULL";
			} else {
				try {
					return Convert.ToDecimal(value).ToString();
				} catch {
					return "NULL";
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Customerized composite numeric value</returns>
		public static string GetSQLNumeric(string value) {
			if (string.IsNullOrWhiteSpace(value)) {
				return "NULL";
			} else {
				decimal v;
				if (decimal.TryParse(value,out v)) {
					value = value.Trim();
					int d;
					if (value.IndexOf('.') >= 0) {
						d = value.Substring(value.IndexOf('.')).Length - 1;
					} else {
						d = 0;
					}
					return "'(" + value + ",\"" + value + "\"," + d + ")'";
				} else {
					return "NULL";
				}
			}
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

		public static string Date(object date) {
			if (date.GetType().ToString() == "System.DateTime") {
				string dtformat = string.Empty;
				if (HttpContext.Current.Session["dtformat"] != null) dtformat = HttpContext.Current.Session["dtformat"].ToString();
				if (string.IsNullOrWhiteSpace(dtformat)) dtformat = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];

				return ((DateTime)date).ToString(dtformat);
			} else {
				return string.Empty;
			}
		}
		public static string Date(string date) {
			string dtformat = string.Empty;
			if (HttpContext.Current.Session["dtformat"] != null) dtformat = HttpContext.Current.Session["dtformat"].ToString();
			if (string.IsNullOrWhiteSpace(dtformat)) dtformat = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];

			// remove time
			if (date.Contains(" ")) date = date.Substring(0, date.IndexOf(" "));

			string[] dt = date.Split('-');
			switch (dt.Length) {
				case 1: return date;
				case 2:
					switch (dtformat) {
						case "yyyy-MM-dd": return dt[0] + "-" + dt[1];
						default: return dt[1] + "/" + dt[0];
					}
				case 3:
					switch (dtformat) {
						case "dd/MM/yyyy": return dt[2] + "/" + dt[1] + "/" + dt[0];
						case "MM/dd/yyyy": return dt[1] + "/" + dt[2] + "/" + dt[0];
						default: return date;
					}
				default: return string.Empty;
			}
		}
		public static string Time(object time) {
			if (time.GetType().ToString() == "System.DateTime") {
				return ((DateTime)time).ToString("HH:mm");
			} else {
				return string.Empty;
			}
		}
		public static string DateTime(object date) {
			if (date.GetType().ToString() == "System.DateTime") {
				string dtformat = string.Empty;
				if (HttpContext.Current.Session["dtformat"] != null) dtformat = HttpContext.Current.Session["dtformat"].ToString();
				if (string.IsNullOrWhiteSpace(dtformat)) dtformat = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];

				return ((DateTime)date).ToString(dtformat + " HH:mm");
			} else {
				return string.Empty;
			}
		}
		public static bool Boolean(object obj) {
			if (obj.GetType().ToString() == "System.Boolean") return (bool)obj;
			else return false;
		}
		public static string SQLDate(string date) {
			if (string.IsNullOrEmpty(date)) return string.Empty;
			else {
				string dtformat = string.Empty;
				if (HttpContext.Current.Session["dtformat"] != null) dtformat = HttpContext.Current.Session["dtformat"].ToString();
				if (string.IsNullOrWhiteSpace(dtformat)) dtformat = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];

				string[] dt;
				if (dtformat == "yyyy-MM-dd") dt = date.Split('-');
				else dt = date.Split('/');

				if (dt.Length == 3) {
					switch (dtformat) {
						case "dd/MM/yyyy": return "'" + dt[2] + "-" + dt[1] + "-" + dt[0] + "'";
						case "MM/dd/yyyy": return "'" + dt[2] + "-" + dt[0] + "-" + dt[1] + "'";
						default: return "'" + date + "'";
					}
				} else {
					int year, month;
					if (dt.Length == 1) {
						if (int.TryParse(date, out year)) return "[" + dt[0] + "-01-01," + (year + 1) + "-01-01)";
						else return "(,)";
					} else {
						if (dtformat == "yyyy-MM-dd") {
							if (int.TryParse(dt[0], out year) && int.TryParse(dt[1], out month)) {
								if (month == 12) return "[" + year + "-" + month + "-01," + (year + 1) + "-01-01)";
								else return "[" + year + "-" + month + "-01," + year + (month + 1) + "-01)";
							} else return "(,)";
						} else {
							if (int.TryParse(dt[1], out year) && int.TryParse(dt[0], out month)) {
								if (month == 12) return "[" + year + "-" + month + "-01," + (year + 1) + "-01-01)";
								else return "[" + year + "-" + month + "-01," + year + (month + 1) + "-01)";
							} else return "(,)";
						}
					}
				}
			}
		}
		public static string SQLDate(string start, string end) {
			start = FormatDate(start);
			end = FormatDate(end);

			if (string.IsNullOrWhiteSpace(start)) {
				if (string.IsNullOrWhiteSpace(end)) return "(,)";
				else return "(," + end + "]";
			} else if (string.IsNullOrWhiteSpace(end)) {
				return "[" + start + ",)";
			} else {
				return "[" + start + "," + end + "]";
			}
		}
		public static string SQLDateTime(string date, string time) {
			if (string.IsNullOrWhiteSpace(time)) return FormatDate(date);
			else return FormatDate(date) + " " + time;
		}
		public static void AddInteger(Npgsql.NpgsqlCommand cmd, string field, string value) {
			Int64 v;
			if (Int64.TryParse(value, out v)) {
				cmd.Parameters.AddWithValue(field, v);
			} else {
				cmd.Parameters.AddWithValue(field, DBNull.Value);
			}
		}
		public static void AddUUID(Npgsql.NpgsqlCommand cmd, string field, string value) {
			if (string.IsNullOrWhiteSpace(value)) {
				cmd.Parameters.AddWithValue(field, DBNull.Value);
			} else {
				cmd.Parameters.AddWithValue(field, new Guid(value));
			}
		}

		private static string FormatDate(string date) {
			if (string.IsNullOrEmpty(date)) return string.Empty;
			else {
				string dtformat = string.Empty;
				if (HttpContext.Current.Session["dtformat"] != null) dtformat = HttpContext.Current.Session["dtformat"].ToString();
				if (string.IsNullOrWhiteSpace(dtformat)) dtformat = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];

				if (dtformat != "yyyy-MM-dd") {
					string[] dt = date.Split('/');
					switch (dtformat) {
						case "dd/MM/yyyy": return dt[2] + "-" + dt[1] + "-" + dt[0];
						default: return dt[2] + "-" + dt[0] + "-" + dt[1];
					}
				} else return date;
			}
		}

		public static string GetNewGUID() {
			return "'" + System.Guid.NewGuid().ToString() + "'";
		}
		public static string GetNewKey() {
			return System.Configuration.ConfigurationManager.AppSettings["SiteID"] + ((System.DateTime.UtcNow.Ticks - 621355968000000000) / 10000).ToString("000000000000000");
		}

		public static string ConnectionStringBuilder(string server, string port, string database, string username, string password) {
			return "Server=" + server + ";Database=" + database + ";User ID=" + username + ";Password=" + password + ";Timeout=20;";
		}
		public static string ConnectionStringBuilder(string server, string database, string username, string password) {
			return ConnectionStringBuilder(server, "5432", database, username, password);
		}
	}
}
