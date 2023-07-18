namespace Imobiliare.UI.Utils.UrlSeoFormatters
{
  using Imobiliare.Entities;

  using Imobiliare.UI.BusinessLayer;

  public static class UrlBuilder
  {
    public static string BuildAnuntPageUrl(string numeLocalitate, TipProprietate tipProprietate, TipOfertaGen tipOfertaGen, string titluAnunt, int anuntId)
    {
      var finalUrl = "/Localitate-" + @HtmlHelpers.RemoveSpecialCharacters(numeLocalitate, '-', false) + "/"
                     + @TipOfertaDisplayFormatter.GetDisplayStringForUrl(tipProprietate, tipOfertaGen) + "/"
                     + @HtmlHelpers.RemoveSpecialCharacters(titluAnunt, '-', true) + "/" + anuntId;
      return finalUrl;
    }

    public static string BuildLocalitateUrl(string numeLocalitate, string numeJudet)
    {
      var finalUrl = "/Anunturi/Lista/judet-" + numeJudet +
                     "/localitate-" + HtmlHelpers.RemoveSpecialCharacters(numeLocalitate, '-', false);
      return finalUrl;
    }
  }
}