using System;
using System.Collections.Generic;
using System.Linq;

namespace Imobiliare.UI.BusinessLayer
{
  using Imobiliare.Entities;

  public static class TipVanzatorDisplayFormatter
  {
    private static Dictionary<TipVanzator, string> singularDisplayStrings = new Dictionary<TipVanzator, string>()
                                                                       {
                                                                         {TipVanzator.TotiVanzatorii, "Toți Vânzătorii"},
                                                                         {TipVanzator.PersoanaFizica, "Persoană Fizică"},
                                                                         {TipVanzator.AgentieImobiliara, "Agenție Imobiliară"},
                                                                         {TipVanzator.Constructor, "Constructor"},
                                                                       };

    private static Dictionary<TipVanzator, string> pluralDisplayStrings = new Dictionary<TipVanzator, string>()
                                                                       {
                                                                         {TipVanzator.TotiVanzatorii, "Toți Vânzătorii"},
                                                                         {TipVanzator.PersoanaFizica, "Persoane Fizice"},
                                                                         {TipVanzator.AgentieImobiliara, "Agenții Imobiliare"},
                                                                         {TipVanzator.Constructor, "Constructori"},
                                                                       };

    //url fara diacritice deocamdata
    private static Dictionary<TipVanzator, string> singularDisplayStringsUrlStyle = new Dictionary<TipVanzator, string>()
                                                                       {
                                                                         {TipVanzator.TotiVanzatorii, "Toti-Vanzatorii"},
                                                                         {TipVanzator.PersoanaFizica, "Persoana-Fizica"},
                                                                         {TipVanzator.AgentieImobiliara, "Agentie-Imobiliara"},
                                                                         {TipVanzator.Constructor, "Constructor"},
                                                                       };

    private static Dictionary<TipVanzator, string> pluralDisplayStringsUrlStyle = new Dictionary<TipVanzator, string>()
                                                                                {
                                                                         {TipVanzator.TotiVanzatorii, "Toți-Vanzatorii"},
                                                                         {TipVanzator.PersoanaFizica, "Persoane-Fizice"},
                                                                         {TipVanzator.AgentieImobiliara, "Agentii-Imobiliare"},
                                                                         {TipVanzator.Constructor, "Constructori"},
                                                                                };


    public static string GetDisplayString(TipVanzator tipVanzator)
    {
      return singularDisplayStrings[tipVanzator];
    }

    public static string GetDisplayStringPlural(TipVanzator tipVanzator)
    {
      return pluralDisplayStrings[tipVanzator];
    }

    public static string GetDisplayStringForUrl(TipVanzator tipVanzator)
    {
      return singularDisplayStringsUrlStyle[tipVanzator];
    }

    public static string GetDisplayStringForUrlPlural(TipVanzator tipVanzator)
    {
      return pluralDisplayStringsUrlStyle[tipVanzator];
    }

    public static TipVanzator GetTipOfertaFromString(string tipVanzatorString)
    {
      TipVanzator parsedTipVanzator;
      var parseSucceeded = Enum.TryParse<TipVanzator>(tipVanzatorString, true, out parsedTipVanzator);
      if (parseSucceeded)
      {
        return parsedTipVanzator;
      }

      if (singularDisplayStringsUrlStyle.Any(x => x.Value == tipVanzatorString))
      {
        return singularDisplayStringsUrlStyle.Single(x => x.Value == tipVanzatorString).Key;
      }

      return pluralDisplayStringsUrlStyle.Single(x => x.Value == tipVanzatorString).Key;
    }
  }
}