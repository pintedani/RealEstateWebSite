using System;
using System.Collections.Generic;
using System.Text;

namespace Imobiliare.Managers.ExtensionMethods
{
    public static class StringExtensions
  {

    /// <summary>
    /// Parses a string into an Enum
    /// </summary>
    /// <typeparam name="T">The type of the Enum</typeparam>
    /// <param name="value">String value to parse</param>
    /// <returns>The Enum corresponding to the stringExtensions</returns>
    public static T EnumParse<T>(this string value)
    {
      return StringExtensions.EnumParse<T>(value, false);
    }

    public static T EnumParse<T>(this string value, bool ignorecase)
    {

      if (value == null)
      {
        throw new ArgumentNullException("value");
      }

      value = value.Trim();

      if (value.Length == 0)
      {
        throw new ArgumentException("Must specify valid information for parsing in the string.", "value");
      }

      Type t = typeof(T);

      if (!t.IsEnum)
      {
        throw new ArgumentException("Type provided must be an Enum.", t.ToString());
      }

      return (T)Enum.Parse(t, value, ignorecase);
    }

    public static int ParseToInt(this string value)
    {
      return int.Parse(value);
    }

    public static int TryParseToInt(this string value)
    {
      int parsedValue;
      int.TryParse(value, out parsedValue);
      return parsedValue;
    }

    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

        private static readonly Dictionary<char, char> specialCharacterMappings = new Dictionary<char, char>()
    {
      {'Ă','A' },
      {'ă','a' },
      {'Ș','S' },
      {'ș','s' },
      {'Î','I' },
      {'î','i' },
      {'Ț','T' },
      {'ț','t' },
      {'â','a' },
      {'Â', 'A' }

    };

    private static List<char> allowedChars = new List<char>
    {
      //'.',
      //','
      //':' error potentially dangerous request in url
    };

    //The same as in Imobiliare.UI.BusinessLayer.HtmlHelpers 
    //TODO Unify with other method
    public static string RemoveSpecialCharacters(this string str, char bindingCharacter, bool removeTrailingDot)
    {
      var sb = new StringBuilder();
      foreach (var c in str)
      {
        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == '-' || c == ' ')
        {
          //Add special binging char instead of _ if this is the case
          sb.Append(c != ' ' ? c : bindingCharacter);
        }
        else if (specialCharacterMappings.ContainsKey(c))
        {
          sb.Append(specialCharacterMappings[c]);
        }
        //else if (allowedChars.Contains(c))
        //{
        //  sb.Append(c);
        //}
      }

      //Replace to many spaces with only one 
      //TODO DAPI: Check also in apartamente24.ro?
      sb.Replace("----", "-");
      sb.Replace("---", "-");
      sb.Replace("--", "-");

      return removeTrailingDot ? sb.ToString().TrimEnd('.') : sb.ToString();
    }
  }
}