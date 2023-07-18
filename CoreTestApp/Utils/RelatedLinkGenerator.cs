using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.BusinessLayer
{
  public static class RelatedLinkGenerator
  {
    private static List<string> localitatiGenerate = new List<string>()
                                                     {
                                                       "Cluj",
                                                       "Bucuresti-Ilfov",
                                                       "Timis",
                                                       "Brasov",
                                                       "Iasi",
                                                       "Constanta",
                                                       "Sibiu"
                                                     };

    public static List<string> LocalitatiGenerate
    {
      get
      {
        return localitatiGenerate;
      }
    }

    private static Dictionary<string, string>  linkuriClujNapoca = new Dictionary<string, string>()
                                                                   {
                                                                     {"apartamente2camere","Apartamente vanzare Cluj"},
                                                                     {"",""}
                                                                   };
  }
}