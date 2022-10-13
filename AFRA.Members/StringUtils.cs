using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AFRA.Members;

public static class StringExtensions {
	public static string Delimit(this string input, int max) => input.PadRight(max, ' ')[..max].TrimEnd();
	public static string Delimit(this string input, int max, string suffix) {
		return input.Length > max ? input.PadRight(max, ' ')[..max].TrimEnd() + suffix : input;
	}
}
