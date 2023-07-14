using System.Collections.Generic;

namespace Imobiliare.ServiceLayer.Interfaces
{
  public interface IExternalSiteParserService
  {
    ExternalSourceAnunt GetParsedContent(string url);

    List<string> GetPicturesUrlList(string url);
  }
}
