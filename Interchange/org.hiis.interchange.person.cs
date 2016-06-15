using System;
using System.Collections.Generic;

namespace org.hiis.Interchange {
	public class Person {
		#region Name
		public string FirstName {
			get; set;
		}
		public string Surname {
			get; set;
		}
		public string Name {
			get {
				string tmp = string.Empty;
				if (string.IsNullOrWhiteSpace(Surname)) {
					if (!string.IsNullOrWhiteSpace(FirstName)) {
						tmp = FirstName;
					}
				} else {
					if (string.IsNullOrWhiteSpace(FirstName)) {
						tmp = Surname.ToUpper();
					} else {
						tmp = FirstName + " " + Surname.ToUpper();
					}
				}
				return tmp;
			}
			set { }
		}
		#endregion
		List<Address> Address = new List<Address>();
		#region Other
		public int GetAge(DateTime dob) {
			return GetAge(dob, DateTime.Today);
		}
		public int GetAge(string dob) {
			try {
				DateTime dt = Convert.ToDateTime(dob);
				return GetAge(dt, DateTime.Today);
			} catch (FormatException) {
				return -1;
			}
		}
		public int GetAge(DateTime dob, DateTime now) {
			if (DateTime.Compare(dob, now) < 0) {
				int years = now.Year - dob.Year;
				if (now.Month < dob.Month || (now.Month == dob.Month && now.Day < dob.Day)) {
					--years;
				}
				return years;
			} else {
				return -1;
			}
		}
		public int GetAge(string dob, string now) {
			try {
				DateTime dt = Convert.ToDateTime(dob);
				DateTime nw = Convert.ToDateTime(now);
				return GetAge(dt, nw);
			} catch (FormatException) {
				return -1;
			}
		}
            		#endregion

	}
}
