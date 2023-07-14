namespace Imobiliare.ServiceLayer.Interfaces
{
  public interface IRecaptchaValidator
  {
    CaptchaResponse GetCaptchaResponse(string response);
  }
}
