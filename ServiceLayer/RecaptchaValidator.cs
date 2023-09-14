using System;
using System.Collections.Generic;
using System.Net;
using Imobiliare.ServiceLayer.Interfaces;
using Logging;
using Newtonsoft.Json;

namespace Imobiliare.ServiceLayer
{
    public class RecaptchaValidator : IRecaptchaValidator
  {
    private static readonly ILog log = LogManager.GetLogger(typeof(RecaptchaValidator));

    private const string CAPTCHA_PRIVATE_KEY = "6LdoSQwTAAAAAGlUmRinm6nXyWI1nNfkLKzVc8Qn";

    public CaptchaResponse GetCaptchaResponse(string response)
    {
      var client = new WebClient();
      log.DebugFormat("Verifying captcha with response {0}", response);
      string reply;
      try
      {
        reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", CAPTCHA_PRIVATE_KEY, response));
      }
      catch (Exception exception)
      {
        return new CaptchaResponse()
               {
                 Success = false,
                 ErrorCodes = new List<string> { "Error captcha by connecting to service to DownloadString " + exception.Message }
               };
      }

      var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);
      log.DebugFormat("Captcha reply is {0}", captchaResponse.Success.ToString());
      if (captchaResponse.ErrorCodes != null)
      {
        foreach (var errorCode in captchaResponse.ErrorCodes)
        {
          log.DebugFormat("Captcha errorcode is {0}", errorCode);
        }
      }

      return captchaResponse;
      //when response is false check for the error message
      //if (!captchaResponse.Success)
      //{
      //  if (captchaResponse.ErrorCodes.Count <= 0) return View();

      //  var error = captchaResponse.ErrorCodes[0].ToLower();
      //  switch (error)
      //  {
      //    case ("missing-input-secret"):
      //      ViewBag.CaptchaErrorMessage = "The secret parameter is missing.";
      //      ModelState.AddModelError("secretParamMissing", "The secret parameter is missing.");
      //      break;
      //    case ("invalid-input-secret"):
      //      ViewBag.CaptchaErrorMessage = "The secret parameter is invalid or malformed.";
      //      ModelState.AddModelError("secretParamMissing", "The secret parameter is invalid.");
      //      break;

      //    case ("missing-input-response"):
      //      ViewBag.CaptchaErrorMessage = "The response parameter is missing.";
      //      ModelState.AddModelError("secretParamMissing", "The response parameter is missing.");
      //      break;
      //    case ("invalid-input-response"):
      //      ViewBag.CaptchaErrorMessage = "The response parameter is invalid or malformed.";
      //      ModelState.AddModelError("secretParamMissing", "The response parameter is invalid.");
      //      break;

      //    default:
      //      ViewBag.CaptchaErrorMessage = "Error occured. Please try again";
      //      ModelState.AddModelError("secretParamMissing", "General recaptcha error.");
      //      break;
      //  }
      //}
      //else
      //{
      //  //ViewBag.Message = "Valid";
      //}
    }
  }
}
