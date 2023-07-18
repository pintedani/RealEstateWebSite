using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Imobiliare.UI.BusinessLayer
{
  public static class HtmlHelpers
  {
    private static readonly Dictionary<char,char> specialCharacterMappings = new Dictionary<char, char>()
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
      '.',
      ','
      //':' error potentially dangerous request in url
    };

    //The same as in extension method Imobiliare.Managers.StringExtensions
    //TODO Unify with other method
    /// <summary>
    /// Removes special characters from string
    /// </summary>
    /// <param name="str">the string to format</param>
    /// <param name="bindingCharacter">either "space" or "-"</param>
    /// <param name="removeTrailingDot">Special for anunt url. If anunt ends with . then this gives error in url</param>
    /// <param name="allowPunctuationMarks">For meta description, punctuation marks have to be true, else false is ok</param>
    /// <returns>formatted string</returns>
    public static string RemoveSpecialCharacters(string str, char bindingCharacter, bool removeTrailingDot, bool allowPunctuationMarks = false)
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
        else if (allowPunctuationMarks && allowedChars.Contains(c))
        {
          sb.Append(c);
        }
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