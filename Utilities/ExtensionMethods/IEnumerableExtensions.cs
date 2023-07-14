using System;
using System.Collections.Generic;
using System.Linq;

namespace Imobiliare.Managers.ExtensionMethods
{
    public static class IEnumerableExtensions
  {

    public static IEnumerable<t> Randomize<t>(this IEnumerable<t> target)
    {
      Random r = new Random();

      return target.OrderBy(x => (r.Next()));
    }
  }
}