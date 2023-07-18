using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Utils
{
  using Imobiliare.Entities;

  public static class TipOfertaExtensions
  {
    public static string DisplayValue(this TipProprietate tip)
    {
      switch (tip)
      {
          case TipProprietate.Toate:
          return "Proprietate";
          case TipProprietate.Apartament:
          return "Apartament";
        case TipProprietate.Garsoniera:
          return "Garsoniera";
        case TipProprietate.Casa:
          return "Casa";
        case TipProprietate.Teren:
          return "Teren";
      }
      return "";
    }

    public static string DisplayValue(this TipOfertaGen tip)
    {
      switch (tip)
      {
        case TipOfertaGen.Toate:
          return "Tip";
        case TipOfertaGen.Vanzare:
          return "Vanzare";
        case TipOfertaGen.Inchiriere:
          return "Inchiriere";
        case TipOfertaGen.VreauSaCumpar:
          return "Caut sa cumpar";
        case TipOfertaGen.VreauSaInchiriez:
          return "Caut sa inchiriez";
      }
      return "";
    }
  }
}