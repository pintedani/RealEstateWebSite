using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTestApp.Models
{
  public enum MesajCategorie
  {
    /// <summary>
    /// Mesaj in legatura cu un anut
    /// </summary>
    Anunt = 0,

    /// <summary>
    /// Mesaj in legatura cu un profil al unui user definit
    /// </summary>
    Profil = 1,

    /// <summary>
    /// Mesaj in legatura cu un userprofile, gen agent imobiliar
    /// </summary>
    UserContact = 2
  }

  public class Mesaj : Entity
  {
    public Mesaj()
    {
      this.CreateDateTime = DateTime.Now;
    }

    public string FromUserId { get; set; }

    [ForeignKey(nameof(FromUserId))]
    public UserProfile FromUser { get; set; }

    public string ToUserId { get; set; }

    [ForeignKey(nameof(ToUserId))]
    public UserProfile ToUser { get; set; }

    public string Continut { get; set; }

    public DateTime CreateDateTime { get; set; }

    public DateTime CitireDateTime { get; set; }

    public int? ImobilId { get; set; }

    [ForeignKey(nameof(ImobilId))]
    public Imobil Imobil { get; set; }

    public MesajCategorie Categorie { get; set; }

    //public int? ProfilId { get; set; }

    //[ForeignKey(nameof(ProfilId))]
    //public Profil Profil { get; set; }

    public string UserContactFormId { get; set; }

    [ForeignKey(nameof(UserContactFormId))]
    public UserProfile UserContactForm { get; set; }
  }
}
