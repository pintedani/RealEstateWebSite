using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Imobiliare.Entities;

namespace Imobiliare.UI.Models
{
  public class EditConstructorImobiliarModel
  {
    public string Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string NumeComplet { get; set; }

    public string PhoneNumber { get; set; }

    public bool AbonatLaNewsLetter { get; set; }

    public List<Imobil> Anunturi { get; set; }

    public bool TrustedUser { get; set; }

    public int ConstructorId { get; set; }

    public string ConstructorNume { get; set; }

    public string ConstructorDescriere { get; set; }

    public string ConstructorWebsite { get; set; }

    public string ConstructorTelefon { get; set; }

    public string ConstructorPoza { get; set; }

    public string Picture { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yy HH:mm}")]
    public DateTime? UserCreateDate { get; set; }

    public HttpPostedFileBase ProfileImage { get; set; }
  }
}
