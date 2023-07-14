namespace Imobiliare.ServiceLayer.Interfaces
{
  using Microsoft.Practices.Unity;

  public interface IDependencyResolverCreatorService
  {
    void BuildUnityContainer(IUnityContainer unityContainer);
  }
}
