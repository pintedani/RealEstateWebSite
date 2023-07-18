using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imobiliare.Entities;
using Imobiliare.UI.BusinessLayer;

namespace Imobiliare.UI.Utils.UrlSeoFormatters
{
  public static class AnsambluriRezidentialeUrls
  {
    public static string AnsambluRezidentialUrl(Ansamblu agentie, Oras oras)
    {
      var url = "/Ansamblu-Rezidential";
      if (oras != null)
      {
        url += "/" + HtmlHelpers.RemoveSpecialCharacters(oras.Nume, '-', false);
      }

      if (!string.IsNullOrEmpty(agentie.Titlu))
      {
        url += "/" + HtmlHelpers.RemoveSpecialCharacters(agentie.Titlu, '-', false);
      }

      url += "/" + agentie.Id;
      return url;
    }

    public static string AnsambluriRezidentialeUrl(Oras oras)
    {
      return "/Ansambluri-Rezidentiale/Localitate/" + HtmlHelpers.RemoveSpecialCharacters(oras.Nume, '-', false) + "/" + oras.Id;
    }
  }
}