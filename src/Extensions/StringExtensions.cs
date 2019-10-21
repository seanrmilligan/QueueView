namespace Extensions
{
    public static class StringExtensions
    {

        public static string Truncate(this string str, int length)
        {
            if (str.Length <= length)
                return str;

            const string ellipsis = "...";
            return str.Substring(0, length - ellipsis.Length) + ellipsis;
        }
    }
}
