namespace AFRA.Members.Backend;

public static class StringExtensions {
	public static string Delimit(this string input, int max) => input.PadRight(max, ' ')[..max].TrimEnd();
	public static string Delimit(this string input, int max, string suffix) {
		return input.Length > max ? input.PadRight(max, ' ')[..max].TrimEnd() + suffix : input;
	}
}
