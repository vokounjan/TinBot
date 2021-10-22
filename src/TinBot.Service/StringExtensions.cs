namespace System
{
    public static class StringExtensions
    {
        public static string LowercaseFirstChar(this string s)
        {
            if (!string.IsNullOrEmpty(s) && char.IsUpper(s[0]))
            {
                s = char.ToLower(s[0]) + s[1..];
            }

            return s;
        }
    }
}
