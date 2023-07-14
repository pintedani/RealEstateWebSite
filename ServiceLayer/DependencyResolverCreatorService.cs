using Imobiliare.Managers;
using Imobiliare.Repositories;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.ServiceLayer.EmailService;
using Imobiliare.ServiceLayer.ExternalSiteContentParser;
using Imobiliare.ServiceLayer.Interfaces;
using Microsoft.Practices.Unity;

namespace Imobiliare.ServiceLayer
{
  public class DependencyResolverCreatorService : IDependencyResolverCreatorService
  {
    public void BuildUnityContainer(IUnityContainer container)
    {
      container.RegisterType<IEmailSender, EmailSender>();

      container.RegisterType<IEmailTemplateRepository, EmailTemplateRepository>();
      container.RegisterType<IAnunturiRepository, AnunturiRepository>();

      //Se pare ca merge si fara ContainerControlled, in special pentru caching
      //container.RegisterType<IOptionsRepository, OptionsRepository>(/*new ContainerControlledLifetimeManager()*/);
      container.RegisterType<IJudetRepository, JudetRepository>();
      container.RegisterType<IOrasRepository, OrasRepository>();
      container.RegisterType<ICartierRepository, CartierRepository>();
      container.RegisterType<IUsersRepository, UsersRepository>();
      container.RegisterType<ILogsRepository, LogsRepository>();
      container.RegisterType<ISystemSettingsRepository, SystemSettingsRepository>();
      container.RegisterType<IStiriRepository, StiriRepository>();
      container.RegisterType<IAgentiiRepository, AgentiiRepository>();
      container.RegisterType<IConstructoriRepository, ConstructoriRepository>();
      container.RegisterType<IMesajRepository, MesajRepository>();
      container.RegisterType<IAnsambluriRepository, AnsambluriRepository>();
      container.RegisterType<IAuditTrailRepository, AuditTrailRepository>();

      //Services
      container.RegisterType<IEmailManagerService, EmailManagerService>();

      container.RegisterType<IExternalSiteParserService, ExternalSiteParserService>();
      container.RegisterType<IRecaptchaValidator, RecaptchaValidator>();

      container.RegisterType<IUnitOfWork, UnitOfWork>();
    }
  }
}
