using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Requeceptor.Core;

public static class StringExtensions
{
    /// <summary>
    /// Skrati string na željenu dužinu.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static string? Truncate(this string? value, int maxLength, string ellipses = "...")
    {
        if (string.IsNullOrEmpty(value))
            return value;

        if (value.Length <= maxLength)
            return value;

        return value.Substring(0, maxLength - ellipses.Length) + ellipses;
    }

    /// <summary>
    /// Konvertiraj u string u željeni <b>T</b> tip podatka.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <returns></returns>
    public static T? ConvertTo<T>(this string input)
    {
        try
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                // Cast ConvertFromString(string text) : object to (T)
                return (T?)converter.ConvertFromString(input);
            }
            return default;
        }
        catch (NotSupportedException)
        {
            return default;
        }
    }

    /// <summary>
    /// Vrati <b>null</b> ako je <b>value</b> prazan.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string? NullIfEmpty(this string? value) => string.IsNullOrWhiteSpace(value) ? null : value;

    /// <summary>
    /// Uspoređuje dva stringa ignorirajući razliku između velikigh i malih slova.
    /// </summary>
    /// <param name="me"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool EqualsIgnoreCase(this string? me, string? value) => string.Equals(value, me, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Uzmi <b>length</b> broj znakova s desna.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Right(this string value, int length) => !string.IsNullOrEmpty(value) ? value.Substring(value.Length - length) : value;

    /// <summary>
    /// Uzmi <b>length</b> broj znakova s lijeva.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Left(this string value, int length) => !string.IsNullOrEmpty(value) ? value.Substring(0, Math.Min(length, value.Length)) : value;

    /// <summary>
    /// Ponovi string <b>count</b> puta
    /// </summary>
    /// <param name="value"></param>
    /// <param name="count">broj ponavljanja</param>
    /// <returns></returns>
    public static string Repeat(this string value, int count) => string.Concat(Enumerable.Repeat(value, count));

    /// <summary>
    /// Formatiranje stringa sa parametrima
    /// </summary>
    /// <param name="value"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string WithPlaceholders(this string value, params object?[] args)
    {
        return string.Format(value, args);
    }

    /// <summary>
    /// Stavlja prvo slovo stringa veliko, a sva ostala slova mala
    /// </summary>
    /// <param name="value">string</param>
    /// <returns>string koji ima prvo veliko slovo, a ostala mala</returns>
    public static string FirstUpper(this string value)
    {
        return $"{value[0].ToString().ToUpper()}{value.Substring(1)}";
    }

    public static bool IsUlr(this string value)
    {
        string pattern = @"^(http|https):\/\/[^\s/$.?#].[^\s]*$";

        // Provjera pomoću Regex.IsMatch metode
        bool isUrl = Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase);
        return isUrl;
    }

    public static string RemoveRight(this string str, int length)
    {
        if (length < 1) return "";
        if (length < str.Length) return str.Remove(str.Length - length);
        return str;
    }
    public static string RemoveLeft(this string str, int length)
    {
        if (length < 1) return "";
        if (length < str.Length) return str.Remove(0, length);
        return str;
    }

    /// <summary>
    /// Ukloni određeni dio stringa s desne strane
    /// </summary>
    /// <param name="str"></param>
    /// <param name="part"></param>
    /// <returns></returns>
    public static string RemoveRight(this string str, string part)
    {
        if (!str.EndsWith(part, StringComparison.Ordinal))
        {
            return str;
        }

        return str.RemoveRight(part.Length);
    }
}
