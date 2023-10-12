using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Imobiliare.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Imobiliare.UI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }

    //    [Required]
    //    public string TrustedUser { get; set; }
    //}

    //public class UserApplicationDbContext : IdentityDbContext<UserProfile>
    //{
    //    public UserApplicationDbContext()
    //        : base("DefaultConnection", throwIfV1Schema: false)
    //    {
    //    }

    //    public static UserApplicationDbContext Create()
    //    {
    //        return new UserApplicationDbContext();
    //    }
    //}

    //public class Test
    //{
    //    public Test()
    //    {
    //        UserApplicationDbContext u = new UserApplicationDbContext();
    //        var user = u.Users.First();
    //        user.
    //    }
    //}
}