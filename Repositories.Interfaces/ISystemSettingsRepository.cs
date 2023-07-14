using Imobiliare.Entities;

namespace Imobiliare.Repositories.Interfaces
{
  public interface ISystemSettingsRepository : IRepository<SystemSettings>
  {
    SystemSettings SystemSettings { get; }

    void UpdateSystemSettings(SystemSettings systemSettings);
  }
}
