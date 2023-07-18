using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Imobiliare.ServiceLayer.Interfaces;

namespace Imobiliare.ServiceLayer.ExternalSiteContentParser
{
    public class ExternalSiteParserService : IExternalSiteParserService
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Safari/537.36 Edg/90.0.818.46";
        private static readonly ILog log = LogManager.GetLogger(typeof(ExternalSiteParserService));

        public ExternalSourceAnunt GetParsedContent(string url)
        {
            var externalSourceAnunt = new ExternalSourceAnunt();

            using (var client = new WebClient()) // WebClient class inherits IDisposable
            {
                //Ptr diacritice
                try
                {
                    client.Encoding = Encoding.UTF8;
                    //https://stackoverflow.com/questions/2859790/the-request-was-aborted-could-not-create-ssl-tls-secure-channel
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //put useragent because olx is detecting bot
                    client.Headers[HttpRequestHeader.UserAgent] = UserAgent;
                    string siteContent = client.DownloadString(url);

                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(siteContent);

                    if (siteContent.Contains("olx"))
                    {
                        log.DebugFormat("Se incearca parsarea siteului olx {0}", url);

                        if (siteContent.Contains("- arata telefon -"))
                        {
                            externalSourceAnunt.EroareParsare = "EROARE PARSARE, descrierea contine - arata telefon -!";
                            log.WarnFormat("EROARE PARSARE, descrierea contine - arata telefon -!");
                            return externalSourceAnunt;
                        }

                        ExtrageInformatiiOlxRo(htmlDoc, externalSourceAnunt);
                    }
                    else if (siteContent.Contains("piata-az.ro"))
                    {
                        ExtrageInformatiiPiataAzRo(htmlDoc, externalSourceAnunt);
                    }
                    else
                    {
                        log.WarnFormat("Nu exista parser pentru pagina selectata {0}, continut pagina {1}", url, siteContent);
                    }

                    log.DebugFormat("Parsare url cu succes {0}", url);
                }
                catch (Exception exception)
                {
                    externalSourceAnunt.EroareParsare = "EROARE PARSARE, verificati campurile manual!";
                    log.ErrorFormat($"Eroare la extragere informatii de pe site extern: {exception.Message}");
                }
            }
            return externalSourceAnunt;
        }

        public List<string> GetPicturesUrlList(string url)
        {
            List<string> urlList = new List<string>();

            using (var client = new WebClient()) // WebClient class inherits IDisposable
            {
                client.Headers[HttpRequestHeader.UserAgent] = UserAgent;
                string s = client.DownloadString(url);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(s);

                try
                {
                    //determine site
                    if (s.Contains("olx"))
                    {
                        log.DebugFormat("Se incearca parsarea imaginilor pentru url olx {0}", url);
                        var images = htmlDoc.DocumentNode.SelectNodes("//div[@class='swiper-zoom-container']/img");

                        foreach (var image in images)
                        {
                            var imageAttribute = image.GetAttributeValue("src", null);
                            if (imageAttribute != null)
                            {
                                urlList.Add(imageAttribute);
                            }
                        }
                    }

                    log.DebugFormat("Parsare image url cu succes {0}", url);
                }
                catch (Exception exception)
                {
                    log.ErrorFormat("Eroare la extragere imagini de pe site extern: {0}", exception.Message);
                }
            }

            return urlList;
        }

        private static void ExtrageInformatiiOlxRo(HtmlDocument htmlDoc, ExternalSourceAnunt externalSourceAnunt)
        {
            var titleNodes = htmlDoc.DocumentNode.SelectNodes("//h1[@data-cy='ad_title']");
            var title = titleNodes[0].InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "").Trim();
            externalSourceAnunt.Titlu = title;

            var descriere = htmlDoc.DocumentNode.SelectNodes("//div[@class='css-g5mtbi-Text']")[0].InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "");
            externalSourceAnunt.Descriere = descriere;

            var suprafataList = htmlDoc.DocumentNode.SelectNodes("//*[contains(text(),'Suprafata utila')]");
            var suprafata = int.Parse(GetNumberString(suprafataList[0].InnerText).ToString().TrimEnd('2'));
            externalSourceAnunt.Suprafata = suprafata;

            var price = htmlDoc.DocumentNode.SelectNodes("//div[@data-testid='ad-price-container']/h3");
            externalSourceAnunt.Price = GetNumberString(price[0].InnerText);

            var numarCamereSel2 = htmlDoc.DocumentNode.SelectNodes("//ol[@data-cy='categories-breadcrumbs']/li");
            if (numarCamereSel2 != null)
            {
                externalSourceAnunt.NumarCamere = GetNumberString(numarCamereSel2[3].InnerText);
            }

            //var etaj = GetNumberString(htmlDoc.DocumentNode.SelectNodes("//small[@itemprop='itemEtaj']")[0].InnerText);
            //var decomandatInfo = htmlDoc.DocumentNode.SelectNodes("//*[contains(text(),'Compartimentare:')]/strong/a");
            //if (decomandatInfo != null)
            //{
            //    var decomandat = decomandatInfo[0].InnerText.Replace("\t", "").Replace("\n", "").ToLower();
            //    externalSourceAnunt.Decomandat = decomandat == "decomandat";
            //}
        }

        private static int GetNumberString(string inputString)
        {
            return int.Parse(new string(inputString.ToCharArray().Where(c => char.IsDigit(c)).ToArray()));
        }

        private static void ExtrageInformatiiPiataAzRo(HtmlDocument htmlDoc, ExternalSourceAnunt externalSourceAnunt)
        {
            var title = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='contentwrapper']/h1").InnerText;
            externalSourceAnunt.Titlu = title;
            var descriere = htmlDoc.DocumentNode.SelectSingleNode("//p[@class='descriere-text']").InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "");
            externalSourceAnunt.Descriere = descriere;

            var telefon = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='show-phone-id']/@value").GetAttributeValue("value", "");
            externalSourceAnunt.Telefon = telefon;

            var price = GetNumberString(htmlDoc.DocumentNode.SelectSingleNode("//div[@id='detaliu-pret-mob']").InnerText);
            externalSourceAnunt.Price = price;

            var etajExists = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='Etaj']");
            if (etajExists != null)
            {
                var etaj = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='Etaj']").InnerText;
                if (etaj == "Nespecificat")
                {
                    externalSourceAnunt.Etaj = -4;
                }
                else if (etaj == "Demisol")
                {
                    externalSourceAnunt.Etaj = -3;
                }
                else if (etaj == "Parter")
                {
                    externalSourceAnunt.Etaj = -2;
                }
                else if (etaj == "Mansarda")
                {
                    externalSourceAnunt.Etaj = -2;
                }
                else
                {
                    externalSourceAnunt.Etaj = GetNumberString(etaj);
                }
            }

            var numarCamereExists = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Nr. camere') and @class = 'actiuni-col-a']");
            if (numarCamereExists != null)
            {
                var numarCamere = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(text(),'Nr. camere') and @class = 'actiuni-col-a']/following::div[1]/a").InnerText;
                if (numarCamere != "Nespecificat")
                {
                    externalSourceAnunt.NumarCamere = GetNumberString(numarCamere);
                }
            }

            var suprafataExists = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Suprafata') and @class = 'actiuni-col-a']");
            if (suprafataExists != null)
            {
                var suprafata = GetNumberString(htmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Suprafata') and @class = 'actiuni-col-a']/following::div[1]/a").InnerText).ToString();
                externalSourceAnunt.Suprafata = Int32.Parse(suprafata);
            }

            var compartimExists = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Compartim.') and @class = 'actiuni-col-a']");
            if (compartimExists != null)
            {
                var compartimentare = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Compartim.') and @class = 'actiuni-col-a']/following::div[1]/span").InnerText;
                if (compartimentare == "Decomandat")
                {
                    externalSourceAnunt.Decomandat = true;
                }
            }

            var adresaExists = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Strada') and @class = 'actiuni-col-a']");
            if (adresaExists != null)
            {
                var adresa = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Strada') and @class = 'actiuni-col-a']/following::div[1]/a").InnerText;
                externalSourceAnunt.Adresa = adresa;
            }

            var locParcareExists = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Loc parcare') and @class = 'actiuni-col-a']");
            if (locParcareExists != null)
            {
                var locParcare = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Loc parcare') and @class = 'actiuni-col-a']/following::div[1]").InnerText;
                if (locParcare.Contains("Da"))
                {
                    externalSourceAnunt.LocParcare = true;
                }
            }
        }
    }
}