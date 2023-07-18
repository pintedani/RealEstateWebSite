namespace Imobiliare.UI.BusinessLayer
{
  using Imobiliare.Entities;

  public static class TipOfertaOptions
  {
    /// <summary>
    /// Returns true if option is visible for specified tipoferta
    /// </summary>
    /// <param name="anuntOptiune"></param>
    /// <param name="tipOferta"></param>
    /// <param name="tipOfertaGen"></param>
    /// <returns></returns>
    public static bool OptionVisible(AnuntOptiuni anuntOptiune, TipProprietate tipOferta, TipOfertaGen tipOfertaGen)
    {
      if (tipOferta == TipProprietate.Toate) return true;
      switch (anuntOptiune)
      {
        case AnuntOptiuni.Decomandat:
          return tipOferta == TipProprietate.Apartament;
        case AnuntOptiuni.NumarCamere:
        case AnuntOptiuni.NumarBai:
          return tipOferta == TipProprietate.Apartament || tipOferta == TipProprietate.Casa;
        case AnuntOptiuni.Centralatermica:
        case AnuntOptiuni.AerConditionat:
        case AnuntOptiuni.Garaj:
        case AnuntOptiuni.LocParcare:
        case AnuntOptiuni.AnulConstructiei:
        case AnuntOptiuni.Finisat:
          return tipOferta != TipProprietate.Teren;
        case AnuntOptiuni.Etaj:
        case AnuntOptiuni.NumarBalcoane:
        case AnuntOptiuni.LocInPivnita:
          return tipOferta == TipProprietate.Apartament || tipOferta == TipProprietate.Garsoniera;

        case AnuntOptiuni.Imagini:
        case AnuntOptiuni.LocatiePeHarta:
          return tipOfertaGen != TipOfertaGen.VreauSaCumpar && tipOfertaGen != TipOfertaGen.VreauSaInchiriez;


        default:
          return true;
      }
    }
  }
}