using Microsoft.AspNetCore.Identity;
//using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
//using Microsoft.AspNet.Identity.EntityFramework;

namespace Imobiliare.Entities
{
    public class UserProfile : IdentityUser
    {
        public Role Role { get; set; }

        public TipVanzator TipVanzator { get; set; }

        public string Picture { get; set; }

        public string ConfirmationToken { get; set; }

        public StareUser StareUser { get; set; }

        //A trusted user is allowed to add items without the need to be approved
        public bool TrustedUser { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yy HH:mm}")]
        public DateTime? UserCreateDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yy HH:mm}")]
        public DateTime? LastLoginTime { get; set; }

        public string Flags { get; set; }

        public bool AbonatLaNewsLetter { get; set; } = true;

        public bool RestrictionatPrimireEmail { get; set; }

        public List<Imobil> Anunturi { get; set; }

        public string NumeComplet { get; set; }

        public bool ReceiveAdminReports { get; set; }


        #region Agentie properties

        [StringLength(40, ErrorMessage = "Numele agentiei nu trebuie sa depaseasca 40 de caractere")]
        public string NumeAgentieImobiliara { get; set; }

        public Agentie Agentie { get; set; }

        [ForeignKey(nameof(Agentie))]
        public int? AgentieId { get; set; }

        public bool IsAgentieAdmin { get; set; }

        public List<UserRating> UserRatings { get; set; }

        public string DescriereAgent { get; set; }

        #endregion


        #region Constructor properties

        public Constructor Constructor { get; set; }

        [ForeignKey(nameof(Constructor))]
        public int? ConstructorId { get; set; }

        #endregion

        public bool IsAdministrator
        {
            get
            {
                return Role == Role.Administrator;
            }
        }


        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserProfile> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom user claims here
        //    return userIdentity;
        //}
    }
}
