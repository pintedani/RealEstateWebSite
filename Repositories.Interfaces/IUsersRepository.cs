namespace Imobiliare.Repositories.Interfaces
{
  using System.Collections.Generic;
  using System.Web;
  using Imobiliare.Entities;
    using Microsoft.AspNetCore.Http;

    public interface IUsersRepository : IRepository<UserProfile>
  {
    List<UserProfile> GetUserProfiles(TipVanzator tipVanzator, Role role, int page, int pagesize, out int totalNumberOfEntries);

    List<UserProfile> GetUserProfiles();

    void UpdateLastLoginTime(string username);

    UserProfile GetUserProfileById(string userProfileId, bool includeRelatedEntities);

    void UpdateConfirmationToken(string userId, string confirmationToken);

        //string AddImageForUserProfile(IFormFile file, string userId);

        string AddImageForAgentie(IFormFile file, int agentieId);

        void AddImageForConstructor(IFormFile profileImage, int constructorId);

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
