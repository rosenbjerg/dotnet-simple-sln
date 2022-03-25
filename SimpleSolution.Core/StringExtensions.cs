using System;
using System.IO;
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
        var hash = GetStableHashCode(input);
        var bytes = Encoding.ASCII.GetBytes(hash.ToString());
        var newArray = new byte[16];
        var startAt = newArray.Length - bytes.Length;
        Array.Copy(bytes, 0, newArray, startAt, bytes.Length);
        return new Guid(newArray);
    }

    // Taken from https://stackoverflow.com/a/36846609
    public static int GetStableHashCode(this string str)
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