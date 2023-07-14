using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Entities
{
  public class UserRating : Entity
  {
    public string Comment { get; set; }

    public int Nota { get; set; }

    public string NumeEvaluator { get; set; }

    public UserRatingStatus RatingCategory { get; set; }
  }

  public enum UserRatingStatus
  {
    InAsteptare = 0,
    Aprobat = 1,
    Respins = 2
  }
}
