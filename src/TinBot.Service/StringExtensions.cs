namespace System
{
    public static class StringExtensions
    {
        public static string LowercaseFirstChar(this string s)
        {
            if (s != string.Empty && char.IsUpper(s[0]))
            {
                s = char.ToLower(s[0]) + s.Substring(1);
            }

            return s;
        }
    }
}
