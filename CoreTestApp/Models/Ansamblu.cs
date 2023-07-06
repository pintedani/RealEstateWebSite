using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTestApp.Models
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Ansamblu : Entity
  {
    [Required]
    public string Titlu { get; set; }

    [Required]
    public string Continut { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public UserProfile UserProfile { get; set; }

    [ForeignKey(nameof(UserProfile))]
    public string UserId { get; set; }

    public int NumarVizualizari { get; set; }

    public string Poze { get; set; }

    public bool Active { get; set; }

    public string Keywords { get; set; }

    public Oras Oras { get; set; }

    [ForeignKey(nameof(Oras))]
    public int OrasSelect { get; set; }

    public double GoogleMarkerCoordinateLat { get; set; }

    public double GoogleMarkerCoordinateLong { get; set; }

    //deprecated, not used in Db
    [NotMapped]
    public string GoogleMarkerCoordinates
    {
      get
      {
        if (this.GoogleMarkerCoordinateLat == 0)
        {
          return null;
        }
        else
        {
          return "(" + this.GoogleMarkerCoordinateLat + ", " + this.GoogleMarkerCoordinateLong + ")";
        }
      }

      set
      {
        if (value == null)
        {
          this.GoogleMarkerCoordinateLat = 0;
          this.GoogleMarkerCoordinateLong = 0;
        }
        else
        {
          this.GoogleMarkerCoordinateLat = double.Parse(value.Split(',')[0].Replace("(", ""));
          this.GoogleMarkerCoordinateLong = double.Parse(value.Split(',')[1].Replace(")", "").Replace(" ", ""));
        }
      }
    }
  }
}