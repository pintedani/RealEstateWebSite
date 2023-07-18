using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imobiliare.Entities;

namespace Imobiliare.UI.Utils.UrlSeoFormatters
{
  using Imobiliare.UI.BusinessLayer;

  public class StiriUrls
  {
    public static string BuildStirePageUrl(Stire stire)
    {
      string finalUrl = String.Empty;
      if (!string.IsNullOrEmpty(stire.TitluUrl))
      {
        finalUrl = "/Stire/" + stire.TitluUrl + "/" + stire.Id;
      }
      else
      {
        finalUrl = "/Stire/" + @HtmlHelpers.RemoveSpecialCharacters(stire.Titlu, '-', true) + "/" + stire.Id;
      }
      return finalUrl;
    }
  }
}