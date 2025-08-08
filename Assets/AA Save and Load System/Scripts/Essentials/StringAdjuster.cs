using System.IO;
using System.Linq;

namespace AASave
{
    public static class StringAdjuster
    {
        public static bool IsStringEmpty(string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        public static bool DoesStringStartsWithNumber(string value)
        {
            return char.IsDigit(value[0]);
        }

        public static bool ContainsInvalidFileNameChars(string value)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();

            foreach ( char c in value )
            {
                if ( invalidChars.Contains(c) )
                {
                    return true;
                }
            }

            return false;
        }

    }
}
