namespace Imobiliare.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Web;

    using Imobiliare.Entities;
    using Imobiliare.Repositories.Interfaces;
    using Logging;

    public class UsersRepository : Repository<UserProfile>, IUsersRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UsersRepository));

        public UsersRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(UserProfile.Id)))
        {
        }

        public List<UserProfile> GetUserProfiles(TipVanzator tipVanzator, Role role, int page, int pagesize, out int totalNumberOfEntries)
        {
            var queryWithIncludes = this.DbContext.Users.Include(nameof(UserProfile.Anunturi));
            var queryAll = queryWithIncludes.OrderByDescending(x => x.UserCreateDate)
                .Where(x => (x.TipVanzator == tipVanzator || tipVanzator == TipVanzator.TotiVanzatorii) && (x.Role == role || role == Role.Toti));

            totalNumberOfEntries = queryAll.Count();
            return queryAll.Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToList();
        }

        //Todo: remove getall and replace with find
        public List<UserProfile> GetUserProfiles()
        {
            List<UserProfile> userProfiles;
            userProfiles =
              this.DbContext.Users.Include(nameof(UserProfile.Anunturi)).ToList();

            return userProfiles;
        }

        //TODO: Not called from anywhere
        public void UpdateLastLoginTime(string username)
        {
            var user = this.DbContext.Users.Single(x => x.UserName == username);
            user.LastLoginTime = DateTime.Now;
            this.DbContext.Entry(user).State = EntityState.Modified;
        }

        public UserProfile GetUserProfileById(string userProfileId, bool includeRelatedEntities)
        {
            UserProfile userProfile;
            var query = this.DbContext.Users;

            //TODO DAPI: Include seems not to work, check reason
            if (includeRelatedEntities)
            {
                query.Include(nameof(UserProfile.Agentie));
                query.Include(nameof(UserProfile.Anunturi));
                query.Include(nameof(UserProfile.Constructor));
            }

            userProfile = query.SingleOrDefault(x => x.Id == userProfileId);
            //TODO Fix with include, check why it s not working
            if (userProfile.AgentieId != null)
            {
                userProfile.Agentie = this.DbContext.Agenties.Include(nameof(Agentie.AgentieImobiliaraUserProfiles)).SingleOrDefault(x => x.Id == userProfile.AgentieId);
            }
            if (userProfile.ConstructorId != null)
            {
                userProfile.Constructor = this.DbContext.Constructori.Include(nameof(Constructor.ConstructorUserProfiles)).SingleOrDefault(x => x.Id == userProfile.ConstructorId);
            }
            return userProfile;
        }

        public UserProfile GetUserProfileByUserName(string userName)
        {
            UserProfile userProfile;
            userProfile = this.DbContext.Users.FirstOrDefault(x => x.UserName == userName);

            return userProfile;
        }

        public void UpdateConfirmationToken(string userId, string confirmationToken)
        {
            var user = this.DbContext.Users.Single(x => x.Id == userId);
            user.ConfirmationToken = confirmationToken;
            this.DbContext.Entry(user).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Modified;
        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            var user = this.DbContext.Users.Single(u => u.Id == userProfile.Id);
            //user.Email = editUserDetailsModel.UserProfile.Email;
            user.PhoneNumber = userProfile.PhoneNumber;
            user.TipVanzator = userProfile.TipVanzator;
            user.Role = userProfile.Role;
            user.LastLoginTime = DateTime.Now;
            user.NumeAgentieImobiliara = userProfile.NumeAgentieImobiliara;
            user.NumeComplet = userProfile.NumeComplet;
            user.AbonatLaNewsLetter = userProfile.AbonatLaNewsLetter;
        }

        public void UpdateUserProfileByAdmin(UserProfile userProfile)
        {
            var user = this.DbContext.Users.Single(u => u.Id == userProfile.Id);
            //user.Email = editUserDetailsModel.UserProfile.Email;
            user.PhoneNumber = userProfile.PhoneNumber;
            user.TipVanzator = userProfile.TipVanzator;
            user.Role = userProfile.Role;
            user.LastLoginTime = DateTime.Now;
            user.TrustedUser = userProfile.TrustedUser;
            user.Flags = userProfile.Flags;
            user.NumeAgentieImobiliara = userProfile.NumeAgentieImobiliara;
            user.NumeComplet = userProfile.NumeComplet;
            user.AbonatLaNewsLetter = userProfile.AbonatLaNewsLetter;
            user.ReceiveAdminReports = userProfile.ReceiveAdminReports;
            user.RestrictionatPrimireEmail = userProfile.RestrictionatPrimireEmail;
            //AgenmtieId nullable int
            if (userProfile.AgentieId != null && userProfile.AgentieId != 0)
            {
                user.Agentie = this.DbContext.Agenties.Single(x => x.Id == userProfile.AgentieId);
            }
        }

        public void AddCustomNoteToUser(string id, string note)
        {
            var itemToApprove = this.DbContext.Users.First(x => x.Id == id);
            itemToApprove.Flags += " | " + note;
        }

        //public string AddImageForUserProfile(HttpPostedFileBase file, string userId)
        //{
        //    try
        //    {
        //        log.DebugFormat("Adding image {0} for user profile {1}", file.FileName, userId);
        //        string pictureName = Guid.NewGuid() + ".jpg";

        //        string path = string.Empty;
        //        //HttpContext.Current may be null
        //        //Use HttpContext for editing of photo and AppDomainAppPath when registering because first one is null then
        //        //TODO DAPI: Maybe HttpRuntime.AppDomainAppPath is better and seems to work for both
        //        if (System.Web.HttpContext.Current != null)
        //        {
        //            path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/profileImages"), pictureName);
        //        }
        //        else
        //        {
        //            path = Path.Combine(HttpRuntime.AppDomainAppPath, "Images/profileImages/" + pictureName);
        //        }

        //        Image image = Image.FromStream(file.InputStream, true, true);
        //        var resizedImage = ResizeImageFixedHeight(image, 240);
        //        var objBitmap = new Bitmap(resizedImage);
        //        //var objBitmap = new Bitmap(Image.FromStream(file.InputStream, true, true), new Size(320, 240));
        //        objBitmap.Save(path, ImageFormat.Jpeg);

        //        var user = this.DbContext.Users.First(x => x.Id == userId);
        //        if (user.Picture != null)
        //        {
        //            this.DeletePhoto(Path.Combine(HttpContext.Current.Server.MapPath("~/Images/profileImages"), user.Picture));
        //            log.DebugFormat("Replace initially image {0} with picture {1} for user profile id {2}", user.Picture, file.FileName, userId);
        //        }
        //        user.Picture = pictureName;

        //        return pictureName;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.ErrorFormat("Eroare la adaugare imagine pentru userprofile, eroarea: {0}", ex.Message);
        //        return null;
        //    }
        //}

        //public string AddImageForAgentie(HttpPostedFileBase file, int agentieId)
        //{
        //    try
        //    {
        //        log.DebugFormat("Adding image {0} for agentie {1}", file.FileName, agentieId);
        //        string pictureName = Guid.NewGuid() + ".jpg";

        //        string path = string.Empty;
        //        //HttpContext.Current may be null
        //        //Use HttpContext for editing of photo and AppDomainAppPath when registering because first one is null then
        //        //TODO DAPI: Maybe HttpRuntime.AppDomainAppPath is better and seems to work for both
        //        if (System.Web.HttpContext.Current != null)
        //        {
        //            path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/LogoAgentii"), pictureName);
        //        }
        //        else
        //        {
        //            path = Path.Combine(HttpRuntime.AppDomainAppPath, "Images/LogoAgentii/" + pictureName);
        //        }

        //        Image image = Image.FromStream(file.InputStream, true, true);
        //        var resizedImage = ResizeImageFixedHeight(image, 240);
        //        var objBitmap = new Bitmap(resizedImage);
        //        //var objBitmap = new Bitmap(Image.FromStream(file.InputStream, true, true), new Size(320, 240));
        //        objBitmap.Save(path, ImageFormat.Jpeg);

        //        var user = this.DbContext.Agenties.First(x => x.Id == agentieId);
        //        if (user.PozaAgentie != null)
        //        {
        //            this.DeletePhoto(Path.Combine(HttpContext.Current.Server.MapPath("~/Images/LogoAgentii"), user.PozaAgentie));
        //            log.DebugFormat("Replace initially image {0} with picture {1} for agentie id {2}", user.PozaAgentie, file.FileName, agentieId);
        //        }
        //        user.PozaAgentie = pictureName;

        //        return pictureName;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.ErrorFormat("Eroare la adaugare imagine pentru agentie, eroarea: {0}", ex.Message);
        //        return null;
        //    }
        //}

        //public void AddImageForConstructor(HttpPostedFileBase file, int constructorId)
        //{
        //    try
        //    {
        //        log.DebugFormat("Adding image {0} for constructor {1}", file.FileName, constructorId);
        //        string pictureName = Guid.NewGuid() + ".jpg";

        //        string path = string.Empty;
        //        //HttpContext.Current may be null
        //        //Use HttpContext for editing of photo and AppDomainAppPath when registering because first one is null then
        //        //TODO DAPI: Maybe HttpRuntime.AppDomainAppPath is better and seems to work for both
        //        if (System.Web.HttpContext.Current != null)
        //        {
        //            path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/LogoConstructori"), pictureName);
        //        }
        //        else
        //        {
        //            path = Path.Combine(HttpRuntime.AppDomainAppPath, "Images/LogoConstructori/" + pictureName);
        //        }

        //        Image image = Image.FromStream(file.InputStream, true, true);
        //        var resizedImage = ResizeImageFixedHeight(image, 240);
        //        var objBitmap = new Bitmap(resizedImage);
        //        //var objBitmap = new Bitmap(Image.FromStream(file.InputStream, true, true), new Size(320, 240));
        //        objBitmap.Save(path, ImageFormat.Jpeg);

        //        var constructor = this.DbContext.Constructori.First(x => x.Id == constructorId);
        //        if (constructor.Poza != null)
        //        {
        //            this.DeletePhoto(Path.Combine(HttpContext.Current.Server.MapPath("~/Images/LogoConstructori"), constructor.Poza));
        //            log.DebugFormat("Replace initially image {0} with picture {1} for agentie id {2}", constructor.Poza, file.FileName, constructorId);
        //        }
        //        constructor.Poza = pictureName;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.ErrorFormat("Eroare la adaugare imagine pentru constructor, eroarea: {0}", ex.Message);
        //    }
        //}

        public void DezaboneazaNewsletter(string userid)
        {
            var user = this.DbContext.Users.First(x => x.Id == userid);
            user.AbonatLaNewsLetter = false;
        }

        private void DeletePhoto(string path)
        {
            try
            {
                var fileDel = new FileInfo(path);
                if (fileDel.Exists)
                {
                    fileDel.Delete();
                    log.DebugFormat("Deleted photo {0} for agentie(because replaced by new one)", path);
                }
                else
                {
                    log.ErrorFormat("Attempt to remove photo {0} for agentie failed because not exists", path);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error while removing agentie photo {0}, exception {1}", path, ex.Message);
            }
        }

        //private static Image ResizeImageFixedHeight(Image imgToResize, int height)
        //{
        //    int sourceWidth = imgToResize.Width;
        //    int sourceHeight = imgToResize.Height;

        //    float nPercent = ((float)height / (float)sourceHeight);

        //    int destWidth = (int)(sourceWidth * nPercent);
        //    int destHeight = (int)(sourceHeight * nPercent);

        //    Bitmap b = new Bitmap(destWidth, destHeight);
        //    Graphics g = Graphics.FromImage((Image)b);
        //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //    g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        //    g.Dispose();

        //    return (Image)b;
        //}

        public Dictionary<UserProfile, int> GetAnunturiNumberPerUserProfile(List<UserProfile> userProfiles)
        {
            Dictionary<UserProfile, int> userAnuntNumberList = new Dictionary<UserProfile, int>();
            foreach (var userProfile in userProfiles)
            {
                userAnuntNumberList.Add(userProfile, 0);
            }

            foreach (var imobil in this.DbContext.Imobile.Include("UserProfile"))
            {
                if (userAnuntNumberList.Any(x => x.Key.Id == imobil.UserProfile.Id))
                {
                    var oras = userAnuntNumberList.Single(x => x.Key.Id == imobil.UserProfile.Id).Key;
                    userAnuntNumberList[oras]++;
                }
            }
            return userAnuntNumberList;
        }

        public void UpdateAgentieForUser(string userid, string agentieId)
        {
            log.DebugFormat("Updating agentie for user {0} with agentie {1}", userid, agentieId);
            var intAgentieId = Int32.Parse(agentieId);
            var agentie = this.DbContext.Agenties.Single(x => x.Id == intAgentieId);
            var user = this.DbContext.Users.Single(x => x.Id == userid);
            user.Agentie = agentie;
        }

        public void AssociateUserProfileWithAgency(string id, Agentie agentie, bool isAgentieAdmin)
        {
            var user = this.DbContext.Users.Single(x => x.Id == id);
            user.TipVanzator = TipVanzator.AgentieImobiliara;
            user.Agentie = agentie;
            user.IsAgentieAdmin = true;
        }

        public void AssociateUserProfileWithConstructor(string id, Constructor constructor)
        {
            var user = this.DbContext.Users.Single(x => x.Id == id);
            user.TipVanzator = TipVanzator.Constructor;
            user.Constructor = constructor;
        }

        public void DeleteUser(string id)
        {
            //string username = string.Empty;
            //var user = this.DbContext.Users.Single(u => u.Id == id);
            //username = user.UserName;
            ////Delete photo if agentie imobiliara and available
            //if (!string.IsNullOrEmpty(user.Picture))
            //{
            //    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/profileImages"), user.Picture);
            //    this.DeletePhoto(path);
            //}

            //this.DbContext.Users.Remove(user);
        }
    }
}