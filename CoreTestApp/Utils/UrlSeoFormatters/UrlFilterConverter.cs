using System;
using System.Linq;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.UI.BusinessLayer;


namespace Imobiliare.UI.Utils.UrlSeoFormatters
{
    using Imobiliare.Entities;
    using Imobiliare.Managers.ExtensionMethods;
    using Logging;

    public static class UrlFilterConverter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UrlFilterConverter));

        public static string GenerateUrlString()
        {
            return string.Empty;
        }

        public static RouteValueDictionary GetRouteValueDictionary(ImobilFilter imobilFilter, IUnitOfWork unitOfWork)
        {
            var routeValueDictionary = new RouteValueDictionary();
            //routeValueDictionary.Add("judetId", imobilFilter.JudetId);
            if (imobilFilter.JudetId != 0)
            {
                var judet = unitOfWork.JudetRepository.Single(x => x.Id == imobilFilter.JudetId);
                routeValueDictionary.Add("judet", HtmlHelpers.RemoveSpecialCharacters(judet.Nume, '-', false));
            }
            if (imobilFilter.OrasId != 0)
            {
                var oras = unitOfWork.OrasRepository.Single(x => x.Id == imobilFilter.OrasId);
                routeValueDictionary.Add("localitate", HtmlHelpers.RemoveSpecialCharacters(oras.Nume, '-', false));
            }
            if (imobilFilter.CartierId != 0)
            {
                var cartier = unitOfWork.CartierRepository.Single(x => x.Id == imobilFilter.CartierId);
                routeValueDictionary.Add("cartier", HtmlHelpers.RemoveSpecialCharacters(cartier.Nume, '-', false));
            }
            if (imobilFilter.TipVanzator != 0)
            {
                routeValueDictionary.Add("vanzator", TipVanzatorDisplayFormatter.GetDisplayStringForUrlPlural(imobilFilter.TipVanzator));
            }
            //Disable if to fix bug to not enter wrong state!! In case of !Toate when changing to "Toate" not working with if
            //if (imobilFilter.TipProprietate != 0 || imobilFilter.TipOfertaGen != 0)
            //{
            //    routeValueDictionary.Add("tip", TipOfertaDisplayFormatter.GetDisplayStringForUrlPlural(imobilFilter.TipProprietate, imobilFilter.TipOfertaGen));
            //}

            if (imobilFilter.TipOfertaGen != 0)
            {
                routeValueDictionary.Add("gen", imobilFilter.TipOfertaGen);
            }

            if (imobilFilter.TipProprietate != 0)
            {
                routeValueDictionary.Add("prop", imobilFilter.TipProprietate);
            }

            if (imobilFilter.PretMin != 0 || imobilFilter.PretMax != 0)
            {
                string pret = "0-";
                if (imobilFilter.PretMin != 0)
                {
                    pret = imobilFilter.PretMin + "-";
                }

                if (imobilFilter.PretMax != 0)
                {
                    pret += imobilFilter.PretMax;
                }

                routeValueDictionary.Add("pret", pret);
            }
            if (imobilFilter.MpMin != 0 || imobilFilter.MpMax != 0)
            {
                string mp = "0-";
                if (imobilFilter.MpMin != 0)
                {
                    mp = imobilFilter.MpMin + "-";
                }

                if (imobilFilter.MpMax != 0)
                {
                    mp += imobilFilter.MpMax;
                }

                routeValueDictionary.Add("mp", mp);
            }
            if (imobilFilter.CamereMin != 0 || imobilFilter.CamereMax != 0)
            {
                string camere = "0-";
                if (imobilFilter.CamereMin != 0)
                {
                    camere = imobilFilter.CamereMin + "-";
                }

                if (imobilFilter.CamereMax != 0)
                {
                    camere += imobilFilter.CamereMax;
                }

                routeValueDictionary.Add("camere", camere);
            }
            if (imobilFilter.VechimeMaximaZile != 0)
            {
                routeValueDictionary.Add("vechimezile", imobilFilter.VechimeMaximaZile);
            }
            if (imobilFilter.Page != 1)
            {
                routeValueDictionary.Add("pag", imobilFilter.Page);
            }
            if (!string.IsNullOrEmpty(imobilFilter.MapMode))
            {
                routeValueDictionary.Add("mapmode", imobilFilter.MapMode);
            }
            return routeValueDictionary;
        }

        //Refactor options repository to get from container
        public static void GenerateImobilFilter(IJudetRepository judetRepository, IOrasRepository orasRepository, ICartierRepository cartierRepository, ImobilFilter imobilFilter, string filters)
        {
            var filterCollection = filters.Split('/');
            foreach (var filter in filterCollection)
            {
                if (filter.Contains("judet-"))
                {
                    imobilFilter.JudetId =
                      judetRepository.Judete()
                        .Single(
                          x =>
                            HtmlHelpers.RemoveSpecialCharacters(x.Nume, '-', false).ToLower()
                            == HtmlHelpers.RemoveSpecialCharacters(filter.Replace("judet-", ""), '-', false).ToLower())
                        .Id;
                }
                if (filter.Contains("localitate-") && imobilFilter.JudetId != 0)
                {
                    try
                    {
                        imobilFilter.OrasId =
                          orasRepository.GetSelectableOrases(imobilFilter.JudetId)
                            .Single(
                              x =>
                                HtmlHelpers.RemoveSpecialCharacters(x.Nume, '-', false).ToLower()
                                == HtmlHelpers.RemoveSpecialCharacters(filter.Replace("localitate-", ""), '-', false).ToLower())
                            .Id;
                    }
                    catch(Exception ex)
                    {
                        var filters2 = "";
                        foreach (var filter2 in filters.Split('/'))
                        {
                            filters2 += "|" + filter2;
                        }
                        log.ErrorFormat("Generate imobil filter error, filters: {0}, error {1}", filters2, ex.Message + ex.StackTrace);
                        throw;
                    }
                }
                if (filter.Contains("cartier-"))
                {
                    var cartier = cartierRepository.GetSelectableCartiers(imobilFilter.OrasId)
                      .SingleOrDefault(x => HtmlHelpers.RemoveSpecialCharacters(x.Nume, '-', false).ToLower() == HtmlHelpers.RemoveSpecialCharacters(filter.Replace("cartier-", "").ToLower(), '-', false));

                    //Check null in caz ca se selecteaza un oras cu un cartier dupa care se selecteaza alt oras fara cartiere
                    if (cartier != null)
                    {
                        imobilFilter.CartierId = cartier.Id;
                    }
                }
                //old way with tip
                if (filter.Contains("tip-"))
                {
                    imobilFilter.TipProprietate = TipOfertaDisplayFormatter.GetTipProprietateFromString(filter.Replace("tip-", ""));
                    imobilFilter.TipOfertaGen = TipOfertaDisplayFormatter.GetTipOfertagenFromString(filter.Replace("tip-", ""));
                }
                //new way with gen/prop
                if (filter.Contains("gen-"))
                {
                    imobilFilter.TipOfertaGen = filter.Replace("gen-", "").EnumParse<TipOfertaGen>();
                }
                if (filter.Contains("prop-"))
                {
                    imobilFilter.TipProprietate = filter.Replace("prop-", "").EnumParse<TipProprietate>();
                }
                if (filter.Contains("vanzator-"))
                {
                    imobilFilter.TipVanzator = TipVanzatorDisplayFormatter.GetTipOfertaFromString(filter.Replace("vanzator-", ""));
                }
                if (filter.Contains("pret-"))
                {
                    string pret = filter.Replace("pret-", "");
                    var minMaxSplit = pret.Split('-');

                    imobilFilter.PretMin = int.Parse(minMaxSplit[0]);
                    imobilFilter.PretMax = minMaxSplit[1] != string.Empty ? int.Parse(minMaxSplit[1]) : 0;
                }
                if (filter.Contains("mp-"))
                {
                    //Fix bug when searching for Camp-Moti | BH - here mp- is in filter but no value
                    if (filter.Any(char.IsDigit))
                    {
                        string mp = filter.Replace("mp-", "");
                        var minMaxMp = mp.Split('-');

                        imobilFilter.MpMin = int.Parse(minMaxMp[0]);
                        imobilFilter.MpMax = minMaxMp[1] != string.Empty ? int.Parse(minMaxMp[1]) : 0;
                    }
                }
                if (filter.Contains("camere-"))
                {
                    string camere = filter.Replace("camere-", "");
                    var minMaxCamere = camere.Split('-');

                    imobilFilter.CamereMin = int.Parse(minMaxCamere[0]);
                    imobilFilter.CamereMax = minMaxCamere[1] != string.Empty ? int.Parse(minMaxCamere[1]) : 0;
                }
                if (filter.Contains("vechimezile-"))
                {
                    imobilFilter.VechimeMaximaZile = int.Parse(filter.Replace("vechimezile-", ""));
                }
                if (filter.Contains("pag-"))
                {
                    imobilFilter.Page = int.Parse(filter.Replace("pag-", ""));
                }
                if (filter.Contains("mapmode-"))
                {
                    imobilFilter.MapMode = filter.Replace("mapmode-", "");
                }
            }
        }
    }
}