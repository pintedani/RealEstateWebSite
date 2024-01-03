//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;
//using System.Web.Http.Cors;
//using Imobiliare.Entities;
//using Imobiliare.Repositories.Interfaces;
//using Imobiliare.ServiceLayer.Interfaces;
//using Imobiliare.UI.BusinessLayer;
//using Imobiliare.UI.Models;
//using Imobiliare.UI.Models.DTOs;
//using Imobiliare.UI.Utils.UrlSeoFormatters;
//using log4net;
//using Microsoft.AspNetCore.Cors;

//namespace Imobiliare.UI.Controllers
//{
//    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs
//    public class WebApiAnunturiController : ApiController
//    {
//        private IUnitOfWork unitOfWork;

//        private IExternalSiteParserService externalSiteContentParser;

//        private static readonly ILog log = LogManager.GetLogger(typeof(WebApiAnunturiController));

//        //Needed for web api parameterless ctor
//        public WebApiAnunturiController()
//        {

//        }

//        public WebApiAnunturiController(IUnitOfWork unitOfWork, IExternalSiteParserService externalSiteContentParser)
//        {
//            this.unitOfWork = unitOfWork;
//            this.externalSiteContentParser = externalSiteContentParser;
//        }

//        // GET api/<controller>
//        [HttpGet]
//        [ActionName("GetLatestAnunturi")]
//        public IEnumerable<WebApiAnunt> Get()
//        {
//            try
//            {
//                var webApiAnunts = new List<WebApiAnunt>();
//                var anunturi = unitOfWork.AnunturiRepository.GetLastAddedImobilsOnlyWithPictures(4);
//                foreach (var anunt in anunturi)
//                {
//                    var anuntDto = new WebApiAnunt
//                    {
//                        Titlu = anunt.Title,
//                        Descriere = anunt.Descriere,
//                        Link = Url.Content("~/").TrimEnd('/') + UrlBuilder.BuildAnuntPageUrl(anunt.Oras.Nume, anunt.TipProprietate, anunt.TipOfertaGen, anunt.Title, anunt.Id),
//                        Pret = string.Format("{0:0,0}", anunt.Price),
//                        TipOferta = TipOfertaDisplayFormatter.GetDisplayString(anunt.TipProprietate, anunt.TipOfertaGen),
//                        Oras = anunt.Oras.Nume
//                    };
//                    string poza = null;
//                    if (!string.IsNullOrEmpty(anunt.Poze))
//                    {
//                        poza = Url.Content("~/") + "Images/uploadedPhotos/" + anunt.FirstPhoto;
//                    }
//                    anuntDto.Poza = poza;
//                    webApiAnunts.Add(anuntDto);
//                }
//                return webApiAnunts;

//            }
//            catch (Exception ex)
//            {
//                log.Error($"WebapiAnunturiController api/Get: " + ex.Message);
//                throw;
//            }

//            //return new string[] { "value1", "value2" };
//        }

//        //[HttpPost]
//        [HttpGet]
//        [ActionName("AdaugaAnuntPrinLinkExtern")]
//        public IHttpActionResult AdaugaAnuntPrinLinkExtern([FromUri]LinkExternChromeExtDto linkExternChromeExtDto)
//        {
//            if (string.IsNullOrEmpty(linkExternChromeExtDto.Oras)) LogThrowInvalidArgsException("Oras");
//            if (string.IsNullOrEmpty(linkExternChromeExtDto.Link)) LogThrowInvalidArgsException("Link");
//            if (string.IsNullOrEmpty(linkExternChromeExtDto.EmailAdmin)) LogThrowInvalidArgsException("EmailAdmin");
//            if (string.IsNullOrEmpty(linkExternChromeExtDto.Telefon) || linkExternChromeExtDto.Telefon.Contains("xxx")) LogThrowInvalidArgsException("Telefon = empty or xxx");
//            if (linkExternChromeExtDto.TipOfertaGen == TipOfertaGen.Toate) LogThrowInvalidArgsException("TipOfertaGen = 0");
//            if (linkExternChromeExtDto.TipProprietate == TipProprietate.Toate) LogThrowInvalidArgsException("TipProprietate = 0");
//            if (linkExternChromeExtDto.Valabilitate == 0) LogThrowInvalidArgsException("Valabilitate = 0");

//            SetSecurityContext();

//            try
//            {
//                var link = linkExternChromeExtDto.Link;
//                var user = linkExternChromeExtDto.EmailAdmin;

//                ExternalSourceAnunt externalSourceAnunt = null;
//                var anuntRelatedlink = unitOfWork.AnunturiRepository.GetImobilRelatedToExternalLink(link);
//                if (anuntRelatedlink == null)
//                {
//                    externalSourceAnunt = externalSiteContentParser.GetParsedContent(link);
//                    externalSourceAnunt.LinkExtern = link;
//                }
//                else
//                {
//                    log.Error($"Parsat extern error: anunt link already exist: " + link);
//                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "Anunt already exists!"));
//                }
//                if (externalSourceAnunt.EroareParsare != null)
//                {
//                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, externalSourceAnunt.EroareParsare));
//                }

//                var imobil = new Imobil
//                {
//                    TipVanzator = TipVanzator.PersoanaFizica,

//                    Title = externalSourceAnunt.Titlu,
//                    Descriere = externalSourceAnunt.Descriere,
//                    Suprafata = externalSourceAnunt.Suprafata,
//                    Price = externalSourceAnunt.Price,
//                    NumarCamere = externalSourceAnunt.NumarCamere,
//                    Etaj = externalSourceAnunt.Etaj,
//                    Decomandat = externalSourceAnunt.Decomandat,
//                    Strada = externalSourceAnunt.Adresa,
//                    LocParcare = externalSourceAnunt.LocParcare,
//                    LinkExtern = externalSourceAnunt.LinkExtern,

//                    Oras = unitOfWork.OrasRepository.Find(x => x.Nume == linkExternChromeExtDto.Oras && !x.LocalitateMica).First(),
//                    ContactTelefon = linkExternChromeExtDto.Telefon,
//                    TipProprietate = linkExternChromeExtDto.TipProprietate,
//                    TipOfertaGen = linkExternChromeExtDto.TipOfertaGen,
//                    Valabilitate = linkExternChromeExtDto.Valabilitate,
//                    Negociabil = linkExternChromeExtDto.Negociabil,
//                    CT = linkExternChromeExtDto.CT,
//                    Finisat = linkExternChromeExtDto.Finisat,
//                    ObservatiiAdmin = "Parsat extern full"
//                };

//                var anuntAdaugat = unitOfWork.AnunturiRepository.AddImobil(imobil, user);
//                unitOfWork.Complete();

//                var urlList = externalSiteContentParser.GetPicturesUrlList(link);
//                foreach (var imageUrl in urlList)
//                {
//                    //http://stackoverflow.com/questions/1110246/how-do-i-programatically-save-an-image-from-a-url
//                    var imageRequest = (HttpWebRequest)WebRequest.Create(imageUrl);

//                    //put useragent because olx is detecting bot
//                    imageRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12";
//                    WebResponse imageResponse = imageRequest.GetResponse();

//                    Stream responseStream = imageResponse.GetResponseStream();

//                    HttpPostedFileBase[] httpPostedFileBases = new HttpPostedFileBase[1];
//                    httpPostedFileBases[0] = new MemoryFile(responseStream, "contentType", "name");

//                    unitOfWork.AnunturiRepository.AddImages(anuntAdaugat.Id, httpPostedFileBases);
//                    unitOfWork.Complete();

//                    responseStream.Close();
//                    imageResponse.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error($"Parsat extern error: generic: " + ex.Message);
//            }

//            log.Debug("Parsat extern success: " + linkExternChromeExtDto.Link);
//            return Ok("Anunt parsat cu succes");
//        }

//        private static void SetSecurityContext()
//        {
//            //Needed because on exception on live: Parsat extern error: generic: The request was aborted: Could not create SSL/TLS secure channel.
//            ServicePointManager.Expect100Continue = true;
//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
//        }

//        private void LogThrowInvalidArgsException(string argumentName)
//        {
//            log.Error($"AdaugaAnuntPrinLinkExtern Argument exception: " + argumentName);
//            throw new ArgumentException();
//        }

//        // GET api/<controller>/5
//        public string Get(int id)
//        {
//            return "value";
//        }
//    }
//}