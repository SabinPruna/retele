using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CRC {
    public class Crc {
        public string Message { get; set; }
        public string ExtendedMessage { get; set; }
        public string Polynomial { get; set; }

        public void Read() {
            Console.WriteLine("Insert message:");
            Message = Console.ReadLine();

            Console.WriteLine("Insert polynomial:");
            Polynomial = Console.ReadLine();

        }

        public void Encode() {
            string coef = GetCoeficients();
            int grade = GetPolynomialGrade();
            ExtendedMessage = Message;
            for (int i = 0; i < grade; i++) {
                ExtendedMessage += "0";
            }

            string result = ExtendedMessage;
            while (result.Length >= GetCoeficients().Length) {
                result = ApplyXor(result);
            }

            ExtendedMessage = ExtendedMessage.Remove(ExtendedMessage.Length - result.Length);
            ExtendedMessage += result;
            Console.WriteLine(ExtendedMessage);
        }

        private string ApplyXor(string word) {
            string coef = GetCoeficients();
            string result = string.Empty;
            for (int i = 0; i < coef.Length; i++) {
                result += coef[i] == word[i] ? "0" : "1";
            }

            if (word.Length > result.Length) {
                result += word.Substring(coef.Length);
            }

            result = result.TrimStart("0".ToCharArray());
            return result;
        }

        private int GetPolynomialGrade() {
            int grade = 0;
            if (Polynomial.Contains("x")) {
                grade = 1;
            }

            string[] parts = Polynomial.Split("+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            return (from part in parts
                    select part.SkipWhile(c => !char.IsDigit(c)).TakeWhile(char.IsDigit).ToArray()
                    into digits
                    select new string(digits)
                    into str
                    where str != string.Empty
                    select int.Parse(str)).Concat(new[] {grade}).Max();
        }

        private string GetCoeficients() {
            StringBuilder coefs = new StringBuilder();

            string[] parts = Polynomial.Split("+-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int polynomialGrade = GetPolynomialGrade();

            foreach (string part in parts) {
                while (!part.Contains($"x^{polynomialGrade}") && polynomialGrade > 1) {
                    coefs.Append(0);
                    polynomialGrade--;
                }

                if (part.Contains($"x^{polynomialGrade}")) {
                    coefs.Append(1);
                    polynomialGrade--;
                }

                if (polynomialGrade == 0) {
                    coefs.Append(0);
                    polynomialGrade--;
                }

                if (polynomialGrade == 1) {
                    coefs.Append(part.Equals("x") ? 1 : 0);

                    polynomialGrade--;
                }
            }

            if (polynomialGrade == 1) {
                coefs.Append(0);
                polynomialGrade--;
            }

            if (polynomialGrade == 0) {
                coefs.Append(0);
            }

            return coefs.ToString();
        }
    }
}