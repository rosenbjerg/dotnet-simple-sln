using System.Text;

namespace SimpleSolution.Core;

public static class StringExtensions
{
    public static string TrimNewlines(this string input)
    {
        return input.Trim('\r', '\n');
    }
    public static string AlignDirectorySeparators(this string input)
    {
        return input.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
    }
    public static string FormatGuid(this Guid guid) => guid.ToString("D").ToUpperInvariant();

    public static Guid DeriveGuid(this string input)
    {
        var hash = (long.MaxValue - GetStableHashCode(input)).ToString("X16");
        var bytes = Encoding.UTF8.GetBytes(hash);
        return new Guid(bytes);
    }

    // Taken from https://stackoverflow.com/a/36846609
    private static int GetStableHashCode(this string str)
    {
        unchecked
        {
            var hash1 = 5381;
            var hash2 = hash1;

            for (var i = 0; i < str.Length && str[i] != '\0'; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1 || str[i + 1] == '\0')
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }
}