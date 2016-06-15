namespace org.hiis.Interchange {
	using System;
	using System.Collections.Generic;

	public class Address {
		public string StreetNumber {
			get; set;
		}
		public string StreetName {
			get; set;
		}
		public string StreetType {
			get; set;
		}
		public string Suburb {
			get; set;
		}
		public string Country {
			get; set;
		}

		/// <summary>
		/// Generate JSON format address details
		/// </summary>
		/// <returns></returns>
		public string ToJSON() {
			string tmp = string.Empty;
			return tmp;
		}

		public IDictionary<string, string> COUNTRY = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
			{"AF", "Afghanistan"},
			{"AX", "Åland Islands"},
			{"AL", "Albania"},
			{"DZ", "Algeria"},
			{"AS", "American Samoa"},
			{"AD", "Andorra"},
			{"AO", "Angola"},
			{"AI", "Anguilla"},
			{"AQ", "Antarctica"},
			{"AG", "Antigua and Barbuda"},
			{"AR", "Argentina"},
			{"AM", "Armenia"},
			{"AW", "Aruba"},
			{"AU", "Australia"},
			{"AT", "Austria"},
			{"AZ", "Azerbaijan"},
			{"BS", "Bahamas"},
			{"BH", "Bahrain"},
			{"BD", "Bangladesh"},
			{"BB", "Barbados"},
			{"BY", "Belarus"},
			{"BE", "Belgium"},
			{"BZ", "Belize"},
			{"BJ", "Benin"},
			{"BM", "Bermuda"},
			{"BT", "Bhutan"},
			{"BO", "Bolivia (Plurinational State of)"}
		};
	}

	public enum StreetType {
		Avenue,
		Lane,
		Street
	}

}
