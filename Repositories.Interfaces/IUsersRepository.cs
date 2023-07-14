namespace Imobiliare.Repositories.Interfaces
{
  using System.Collections.Generic;
  using System.Web;
  using Imobiliare.Entities;

  public interface IUsersRepository : IRepository<UserProfile>
  {
    List<UserProfile> GetUserProfiles(TipVanzator tipVanzator, Role role, int page, int pagesize, out int totalNumberOfEntries);

    List<UserProfile> GetUserProfiles();

    void UpdateLastLoginTime(string username);

    UserProfile GetUserProfileById(string userProfileId, bool includeRelatedEntities);

    void UpdateConfirmationToken(string userId, string confirmationToken);

    string AddImageForUserProfile(HttpPostedFileBase file, string userId);

    string AddImageForAgentie(HttpPostedFileBase file, int agentieId);

    void AddImageForConstructor(HttpPostedFileBase profileImage, int constructorId);

    void UpdateUserProfile(UserProfile userProfile);

    void UpdateUserProfileByAdmin(UserProfile userProfile);

    void AddCustomNoteToUser(string id, string note);

    void DezaboneazaNewsletter(string userid);

    Dictionary<UserProfile, int> GetAnunturiNumberPerUserProfile(List<UserProfile> userProfiles);

    void UpdateAgentieForUser(string userid, string agentieId);

    void AssociateUserProfileWithAgency(string id, Agentie agentie, bool isAgentieAdmin);

    void AssociateUserProfileWithConstructor(string id, Constructor constructor);

    void DeleteUser(string id);
  }
}
