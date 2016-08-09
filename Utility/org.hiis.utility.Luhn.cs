namespace org.hiis.Utility {
	public static class Luhn {
		public static string GenerateCheckSum(string id) {
			var sum = 0;
			var alt = true;
			var digits = id.ToCharArray();
			for (int i = digits.Length - 1; i >= 0; i--) {
				var curDigit = (digits[i] - 48);
				if (alt) {
					curDigit *= 2;
					if (curDigit > 9)
						curDigit -= 9;
				}
				sum += curDigit;
				alt = !alt;
			}
			if ((sum % 10) == 0) {
				return "0";
			}
			return (10 - (sum % 10)).ToString();
		}
	}
}
