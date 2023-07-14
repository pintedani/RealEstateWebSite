using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using Microsoft.AspNetCore.Identity;

namespace Imobiliare.Entities
{
    public class Imobil : Entity
    {
        [Required(ErrorMessage = "Completati titlul anuntului")]
        [StringLength(100, ErrorMessage = "Lungimea titlului nu trebuie sa depaseasca 100 de caractere")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Completati descrierea anuntului")]
        [StringLength(5000, ErrorMessage = "Lungimea descrierii nu trebuie sa depaseasca 5000 de caractere")]
        public string Descriere { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0}")]
        [Required(ErrorMessage = "Completati pretul")]
        [Range(1, 50000000, ErrorMessage = "Adaugati o valoare corecta pentru pret")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Pretul trebuie sa contina doar cifre si sa nu inceapa cu 0")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Completati suprafata")]
        [Range(1, 5000000, ErrorMessage = "Adaugati o valoare corecta pentru suprafata < 5000000")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Suprafata Mp trebuie sa contina doar cifre si sa nu inceapa cu 0")]
        public int Suprafata { get; set; }

        public string? Poze { get; set; }

        public Judet Judet { get; set; }

        public Oras Oras { get; set; }

        public Cartier Cartier { get; set; }

        [StringLength(80, ErrorMessage = "Adresa nu trebuie sa depaseasca 80 de caractere")]
        public string? Strada { get; set; }

        public TipOfertaGen TipOfertaGen { get; set; }

        public TipProprietate TipProprietate { get; set; }

        public UserProfile UserProfile { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public string UserId { get; set; }

        //Se modifica la adaugarea anutului si la oricare actualizare
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy HH:mm}")]
        public DateTime DataAdaugare { get; set; }

        //Nu se modifica doar la adaugarea anuntului(vazuta doar de admin)
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy HH:mm}")]
        public DateTime DataAdaugareInitiala { get; set; }

        //Data cand a fost aprobat/ referinta ptr cate zile va fii valabil
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy HH:mm}")]
        public DateTime? DataAprobare { get; set; }

        //Numar zile cat va fii valabil
        public int Valabilitate { get; set; }

        //-4 Nespecificat; -3 Demisol; -2 Parter; -1 Mansarda
        public int Etaj { get; set; }

        public int NumarTotalEtaje { get; set; }

        [RegularExpression("^([0]|[1-2][0-9][0-9][0-9]?)$", ErrorMessage = "Va rugam introduceti un an valid, ex 1995")]
        public int? VechimeImobil { get; set; }

        [RegularExpression("^([0-9][0-9]?)$", ErrorMessage = "Va rugam introduceti un numar de balcoane valid 0-100")]
        public int? NrBalcoane { get; set; }

        [RegularExpression("^([0-9][0-9]?)$", ErrorMessage = "Va rugam introduceti un numar de bai valid 0-100")]
        public int? NrBai { get; set; }

        [RegularExpression("^([0-9][0-9]?)$", ErrorMessage = "Va rugam introduceti un numar de camere valid 0-100")]
        public int? NumarCamere { get; set; }

        [Required(ErrorMessage = "Completati numarul de telefon")]
        public string? ContactTelefon { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail invalid")]
        [StringLength(40, ErrorMessage = "Lungime email nu trebuie sa depaseasca 40 de caractere")]
        public string? ContactEmail { get; set; }

        //Apare daca un anunt a fost adaugat de admin prin parsare site
        public string? LinkExtern { get; set; }

        public int NumarVizualizari { get; set; }

        public StareAprobare StareAprobare { get; set; }

        public double GoogleMarkerCoordinateLat { get; set; }

        public double GoogleMarkerCoordinateLong { get; set; }

        //public TipVanzator TipVanzator { get; set; }

        public bool Garaj { get; set; }

        public bool CT { get; set; }

        public bool AerConditionat { get; set; }

        public bool LocInPivnita { get; set; }

        public bool Negociabil { get; set; }

        public bool LocParcare { get; set; }

        public bool Decomandat { get; set; }

        public bool Finisat { get; set; }

        public PromovatLevel PromotedLevel { get; set; }

        public string? ObservatiiAdmin { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

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
                    return "(" + this.GoogleMarkerCoordinateLat.ToString().Replace(',', '.') + ", " + this.GoogleMarkerCoordinateLong.ToString().Replace(',', '.') + ")";
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
                    this.GoogleMarkerCoordinateLat = double.Parse(value.Split(',')[0].Replace("(", ""), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                    this.GoogleMarkerCoordinateLong = double.Parse(value.Split(',')[1].Replace(")", "").Replace(" ", ""), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                }
            }
        }

        [NotMapped]
        public string FirstPhoto
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Poze))
                {
                    return this.Poze.Split(';')[0];
                }

                switch (this.TipProprietate)
                {
                    case TipProprietate.Apartament:
                        return "imobiliare_apartament.jpg";

                    case TipProprietate.Teren:
                        return "imobiliare_teren.jpg";

                    default:
                        return "imobiliare_no_image.jpg";
                }
            }
        }

        [NotMapped]
        public int NumarPoze
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Poze))
                {
                    return this.Poze.Split(';').Length;
                }
                return 0;
            }
        }
    }

    public enum PromovatLevel
    {
        Nepromovat = 0,
        PromovatNormal = 1,
        SuperPromovat = 2,
        ExceptionalPromovat = 3
    }
}
