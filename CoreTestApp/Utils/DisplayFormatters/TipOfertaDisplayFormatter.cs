using System;
using System.Collections.Generic;
using System.Linq;

namespace Imobiliare.UI.BusinessLayer
{
  using Imobiliare.Entities;

  //ToDo: decide diacritice in URL
  //TODO: rewrite to extension methods, looks more elegand maybe
  public static class TipOfertaDisplayFormatter
  {
    public static string GetDisplayString(TipProprietate tipProprietate, TipOfertaGen tipOfertaGen)
    {
      if (tipOfertaGen == TipOfertaGen.Toate)
      {
        return "Toate Categoriile";
      }

      string tipOfertaGenFinal = string.Empty;

      if (tipOfertaGen == TipOfertaGen.Vanzare)
      {
        tipOfertaGenFinal = "Vânzare ";
      }
      else if (tipOfertaGen == TipOfertaGen.Inchiriere)
      {
        tipOfertaGenFinal = "Chirie ";
      }
      else if (tipOfertaGen == TipOfertaGen.VreauSaCumpar)
      {
        tipOfertaGenFinal = "Caut sa cumpar ";
      }
      else if (tipOfertaGen == TipOfertaGen.VreauSaInchiriez)
      {
        tipOfertaGenFinal = "Caut chirie ";
      }

      if (tipProprietate == TipProprietate.Apartament)
      {
        tipOfertaGenFinal += "Apartament";
      }
      if (tipProprietate == TipProprietate.Garsoniera)
      {
        tipOfertaGenFinal += "Garsonieră";
      }
      if (tipProprietate == TipProprietate.Casa)
      {
        tipOfertaGenFinal += "Casă";
      }
      if (tipProprietate == TipProprietate.Teren)
      {
        tipOfertaGenFinal += "Teren";
      }

      return tipOfertaGenFinal;
    }

    public static string GetDisplayStringPlurals(TipProprietate tipProprietate, TipOfertaGen tipOfertaGen)
    {
      if (tipOfertaGen == TipOfertaGen.Toate)
      {
        return "Toate Categoriile";
      }

      if (tipOfertaGen == TipOfertaGen.Vanzare)
      {
        if (tipProprietate == TipProprietate.Apartament)
        {
          return "Apartamente de Vânzare";
        }
        if (tipProprietate == TipProprietate.Garsoniera)
        {
          return "Garsoniere de Vânzare";
        }
        if (tipProprietate == TipProprietate.Casa)
        {
          return "Case de Vânzare";
        }
        if (tipProprietate == TipProprietate.Teren)
        {
          return "Terenuri de Vânzare";
        }
      }
      else if (tipOfertaGen == TipOfertaGen.Inchiriere)
      {
        if (tipProprietate == TipProprietate.Apartament)
        {
          return "Apartamente de Inchiriat";
        }
        if (tipProprietate == TipProprietate.Garsoniera)
        {
          return "Garsoniere de Inchiriat";
        }
        if (tipProprietate == TipProprietate.Casa)
        {
          return "Case de Inchiriat";
        }
        if (tipProprietate == TipProprietate.Teren)
        {
          return "Terenuri de Inchiriat";
        }
      }
      return string.Empty;
    }

    public static string GetDisplayStringForUrl(TipProprietate tipProprietate, TipOfertaGen tipOfertaGen)
    {
      if (tipProprietate == TipProprietate.Toate)
      {
        switch (tipOfertaGen)
        {
          case TipOfertaGen.Vanzare:
            return "Vanzare";
          case TipOfertaGen.Inchiriere:
            return "Inchiriere";
          case TipOfertaGen.Toate:
            return "Anunturi-Imobiliare";
        }
      }

      if (tipOfertaGen == TipOfertaGen.Toate)
      {
        switch (tipProprietate)
        {
          case TipProprietate.Apartament:
            return "Apartament";
          case TipProprietate.Garsoniera:
            return "Garsoniera";
          case TipProprietate.Casa:
            return "Casa";
          case TipProprietate.Teren:
            return "Teren";
        }
      }

      if (tipOfertaGen == TipOfertaGen.Vanzare || tipOfertaGen == TipOfertaGen.VreauSaCumpar)
      {
        if (tipProprietate == TipProprietate.Apartament)
        {
          return "Vanzare-Apartament";
        }
        if (tipProprietate == TipProprietate.Garsoniera)
        {
          return "Vanzare-Garsoniera";
        }
        if (tipProprietate == TipProprietate.Casa)
        {
          return "Vanzare-Casa";
        }
        if (tipProprietate == TipProprietate.Teren)
        {
          return "Vanzare-Teren";
        }
      }
      else if (tipOfertaGen == TipOfertaGen.Inchiriere || tipOfertaGen == TipOfertaGen.VreauSaInchiriez)
      {
        if (tipProprietate == TipProprietate.Apartament)
        {
          return "Inchiriere-Apartament";
        }
        if (tipProprietate == TipProprietate.Garsoniera)
        {
          return "Inchiriere-Garsoniera";
        }
        if (tipProprietate == TipProprietate.Casa)
        {
          return "Inchiriere-Casa";
        }
        if (tipProprietate == TipProprietate.Teren)
        {
          return "Inchiriere-Teren";
        }
      }
      return string.Empty;
    }

    public static string GetDisplayStringForUrlPlural(TipProprietate tipProprietate, TipOfertaGen tipOfertaGen)
    {
      if (tipProprietate == TipProprietate.Toate)
      {
        switch (tipOfertaGen)
        {
          case TipOfertaGen.Vanzare:
            return "Vanzare";
          case TipOfertaGen.Inchiriere:
            return "Inchiriere";
          case TipOfertaGen.Toate:
            return "Anunturi-Imobiliare";
        }
      }

      if (tipOfertaGen == TipOfertaGen.Toate)
      {
        switch (tipProprietate)
        {
          case TipProprietate.Apartament:
            return "Apartamente";
          case TipProprietate.Garsoniera:
            return "Garsoniere";
          case TipProprietate.Casa:
            return "Case";
          case TipProprietate.Teren:
            return "Terenuri";
        }
      }

      if (tipOfertaGen == TipOfertaGen.Vanzare)
      {
        if (tipProprietate == TipProprietate.Apartament)
        {
          return "Vanzare-Apartamente";
        }
        if (tipProprietate == TipProprietate.Garsoniera)
        {
          return "Vanzare-Garsoniere";
        }
        if (tipProprietate == TipProprietate.Casa)
        {
          return "Vanzare-Case";
        }
        if (tipProprietate == TipProprietate.Teren)
        {
          return "Vanzare-Terenuri";
        }
      }
      else if (tipOfertaGen == TipOfertaGen.Inchiriere)
      {
        if (tipProprietate == TipProprietate.Apartament)
        {
          return "Inchiriat-Apartamente";
        }
        if (tipProprietate == TipProprietate.Garsoniera)
        {
          return "Inchiriat-Garsoniere";
        }
        if (tipProprietate == TipProprietate.Casa)
        {
          return "Inchiriat-Case";
        }
        if (tipProprietate == TipProprietate.Teren)
        {
          return "Inchiriat-Terenuri";
        }
      }
      return string.Empty;
    }

    //public static TipProprietate GetTipProprietateFromString(string tipOfertaString)
    //{
    //  TipProprietate parsedTipOferta;
    //  return TipProprietate.Apartament;
    //  //TODO DAPI Fix this to be like before
    //  //var parseSucceeded = Enum.TryParse<TipOferta>(tipOfertaString, true, out parsedTipOferta);
    //  //if (parseSucceeded)
    //  //{
    //  //  return parsedTipOferta;
    //  //}

    //  //if (singularDisplayStringsUrlStyle.Any(x => x.Value == tipOfertaString))
    //  //{
    //  //  return singularDisplayStringsUrlStyle.Single(x => x.Value == tipOfertaString).Key;
    //  //}

    //  //return pluralDisplayStringsUrlStyle.Single(x => x.Value == tipOfertaString).Key;
    //}

    public static TipProprietate GetTipProprietateFromString(string tipOfertaString)
    {
      if (tipOfertaString.Contains("oate") || tipOfertaString.Contains("Anunturi-Imobiliare"))
      {
        return TipProprietate.Toate;
      }
      if (tipOfertaString.Contains("partament"))
      {
        return TipProprietate.Apartament;
      }
      if (tipOfertaString.Contains("sonier"))
      {
        return TipProprietate.Garsoniera;
      }
      if (tipOfertaString.Contains("asa") || tipOfertaString.Contains("ase"))
      {
        return TipProprietate.Casa;
      }
      if (tipOfertaString.Contains("eren"))
      {
        return TipProprietate.Teren;
      }
      return TipProprietate.Toate;
      //throw new InvalidCastException("tipOfertaString nu este valid in TipOfertaDisplayFormatter!");
    }

    public static TipOfertaGen GetTipOfertagenFromString(string tipOfertaString)
    {
      if (tipOfertaString.Contains("oate") || tipOfertaString.Contains("Anunturi-Imobiliare"))
      {
        return TipOfertaGen.Toate;
      }
      if (tipOfertaString.Contains("zare"))
      {
        return TipOfertaGen.Vanzare;
      }
      if (tipOfertaString.Contains("riere") || tipOfertaString.Contains("chiriat"))
      {
        return TipOfertaGen.Inchiriere;
      }
      return TipOfertaGen.Toate;
      //throw new InvalidCastException("tipOfertaString nu este valid in TipOfertaDisplayFormatter!");
    }
  }
}