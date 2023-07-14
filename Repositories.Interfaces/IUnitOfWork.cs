namespace Imobiliare.Repositories.Interfaces
{
  public interface IUnitOfWork
  {
    IAnunturiRepository AnunturiRepository { get; }
    IUsersRepository UsersRepository { get; }
    IStiriRepository StiriRepository { get; }
    IJudetRepository JudetRepository { get; }
    IOrasRepository OrasRepository { get; }
    ICartierRepository CartierRepository { get; }
    ILogsRepository LogsRepository { get; }
    IEmailTemplateRepository EmailTemplateRepository { get; }
    IConstructoriRepository ConstructoriRepository { get; }
    IAgentiiRepository AgentiiRepository { get; }
    ISystemSettingsRepository SystemSettingsRepository { get; }
    IAuditTrailRepository AuditTrailRepository { get; }
    IBlockedIpRepository BlockedIpRepository { get; }
    IMesajRepository MesajRepository { get; }
    IAnsambluriRepository AnsambluriRepository { get; }
    IRaportActivitateRepository RaportActivitateRepository { get; }
    void Complete();
  }
}