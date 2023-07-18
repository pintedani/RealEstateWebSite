using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Utils.UrlSeoFormatters
{
  using System.Diagnostics.Contracts;

  using Imobiliare.Entities;
  using Imobiliare.UI.BusinessLayer;

  public static class AgentiiImobiliareUrls
  {
    public static string AgentieImobiliaraUrl(Agentie agentie, Oras oras)
    {
      var url = "/Agentie";
      if (oras != null)
      {
        url += "/" + HtmlHelpers.RemoveSpecialCharacters(oras.Nume, '-', false);
      }

      if (!string.IsNullOrEmpty(agentie.Nume))
      {
        url += "/" + HtmlHelpers.RemoveSpecialCharacters(agentie.Nume, '-', false);
      }

      url += "/" + agentie.Id;
      return url;
    }

    public static string AgentiiImobiliaraUrl(Oras oras)
    {
      return "/AgentiiImobiliare/Localitate/" + HtmlHelpers.RemoveSpecialCharacters(oras.Nume, '-', false) + "/" + oras.Id;
    }
  }
}