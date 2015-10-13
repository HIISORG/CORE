using System;
using System.Linq;

namespace org.hiis.Utility {
	public static class Damm {
		#region Define variables
		static int[,] matrix = new int[,] {
			{0, 3, 1, 7, 5, 9, 8, 6, 4, 2},
			{7, 0, 9, 2, 1, 5, 4, 8, 6, 3},
			{4, 2, 0, 6, 8, 7, 1, 3, 5, 9},
			{1, 7, 5, 0, 9, 8, 3, 4, 2, 6},
			{6, 1, 2, 3, 0, 4, 5, 9, 7, 8},
			{3, 6, 7, 4, 2, 0, 9, 5, 8, 1},
			{5, 8, 6, 9, 7, 2, 0, 1, 3, 4},
			{8, 9, 4, 5, 3, 6, 2, 0, 1, 7},
			{9, 4, 3, 8, 6, 1, 7, 2, 0, 5},
			{2, 5, 8, 1, 4, 3, 6, 7, 9, 0}
		};
		#endregion

		/// <summary>
		/// Calculate the checksum digit from provided number
		/// </summary>
		/// <param name="number">the number</param>
		/// <returns>Damm checksum</returns>
		public static int GenerateCheckSum(string number) {
			var numbers = (from n in number select int.Parse(n.ToString()));
			int interim = 0;
			var en = numbers.GetEnumerator();
			while (en.MoveNext()) {
				interim = matrix[interim, en.Current];
			}
			return interim;
		}

		/// <summary>
		/// Calculate the checksum digit from provided number
		/// </summary>
		/// <param name="number">the number</param>
		/// <returns>Damm checksum</returns>
		public static int GenerateCheckSum(int number) {
			return GenerateCheckSum(number.ToString());
		}

		/// <summary>
		/// Calculate the checksum digit from provided number
		/// </summary>
		/// <param name="number">the number</param>
		/// <returns>Damm checksum</returns>
		public static int GenerateCheckSum(long number) {
			return GenerateCheckSum(number.ToString());
		}

		/// <summary>
		/// validates the number using the last digit as the Damm checksum
		/// </summary>
		/// <param name="number">the number to check</param>
		/// <returns>True if valid; otherwise false</returns>
		public static bool Validate(string number) {
			return GenerateCheckSum(number) == 0;
		}

		/// <summary>
		/// validates the number using the last digit as the Damm checksum
		/// </summary>
		/// <param name="number">the number to check</param>
		/// <returns>True if valid; otherwise false</returns>
		public static bool Validate(int number) {
			return GenerateCheckSum(number) == 0;
		}

		/// <summary>
		/// validates the number using the last digit as the Damm checksum
		/// </summary>
		/// <param name="number">the number to check</param>
		/// <returns>True if valid; otherwise false</returns>
		public static bool Validate(long number) {
			return GenerateCheckSum(number) == 0;
		}
	}
}
