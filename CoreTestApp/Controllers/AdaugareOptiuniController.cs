using System;
using System.Linq;
using Imobiliare.Managers.ExtensionMethods;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.Entities;
using Imobiliare.Managers;
using Imobiliare.ServiceLayer.Interfaces;

using Imobiliare.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logging;

namespace Imobiliare.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdaugareOptiuniController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        private static readonly ILog log = LogManager.GetLogger(typeof(AdaugareOptiuniController));

        public AdaugareOptiuniController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



        public ActionResult Index(int? judetIdParam, int? orasIdParam, int? cartierIdParam)
        {
            var judetId = judetIdParam != null ? judetIdParam.Value : 0;

            if (int.TryParse(Request.Form["JudetSelect"], out int selectedJudetId))
            {
                judetId = selectedJudetId;
            }

            var orasId = orasIdParam != null ? orasIdParam.Value : 0;
            if (int.TryParse(Request.Form["OrasSelect"], out int selectedOrasId))
            {
                orasId = selectedOrasId;
            }

            var cartierId = cartierIdParam != null ? cartierIdParam.Value : 0;
            if (int.TryParse(Request.Form["CartierSelect"], out int selectedCartierId))
            {
                cartierId = selectedCartierId;
            }

            var adaugareOptiuniData = new AdaugareOptiuniData();
            adaugareOptiuniData.Judets = this.unitOfWork.JudetRepository.Judete();
            adaugareOptiuniData.SeletedJudet = adaugareOptiuniData.Judets.FirstOrDefault(x => x.Id == judetId) ?? adaugareOptiuniData.Judets.First();

            var imobilFilter = new ImobilFilter { JudetId = adaugareOptiuniData.SeletedJudet.Id, OrasId = orasId };
            adaugareOptiuniData.SelectableOrasesWithNumber = this.unitOfWork.OrasRepository.GetSelectableOrasesWithNumber(imobilFilter);
            adaugareOptiuniData.SelectableCartiersWithNumber = this.unitOfWork.CartierRepository.GetSelectableCartiersWithNumber(imobilFilter);

            //Verifica daca s-a schimbat orasul
            var selectedOras = adaugareOptiuniData.SelectableOrasesWithNumber.Where(x => x.Key.Id == orasId);
            if (selectedOras.Any())
            {
                adaugareOptiuniData.SeletedOras = adaugareOptiuniData.SelectableOrasesWithNumber.Single(x => x.Key.Id == orasId).Key;
            }

            //Verifica daca s-a schimbat orasul/cartierul
            var selectedCartier =
              adaugareOptiuniData.SelectableCartiersWithNumber.Where(
                x => x.Key.Id == cartierId && (adaugareOptiuniData.SeletedOras != null && x.Key.OrasId == adaugareOptiuniData.SeletedOras.Id));
            if (selectedCartier.Any())
            {
                adaugareOptiuniData.SeletedCartier = adaugareOptiuniData.SelectableCartiersWithNumber.Single(x => x.Key.Id == cartierId).Key;
            }

            //Set default oras if no oras selected
            if (adaugareOptiuniData.SeletedOras == null && adaugareOptiuniData.SelectableOrasesWithNumber.Any())
            {
                adaugareOptiuniData.SeletedOras = adaugareOptiuniData.SelectableOrasesWithNumber.First().Key;

                //Schimba cartierele daca s-a modificat orasul
                adaugareOptiuniData.SelectableCartiersWithNumber = this.unitOfWork.CartierRepository.GetSelectableCartiersWithNumber(new ImobilFilter { JudetId = judetId, OrasId = adaugareOptiuniData.SeletedOras.Id });
            }

            //Set default cartier if no cartier selected
            if (adaugareOptiuniData.SeletedCartier == null && adaugareOptiuniData.SelectableCartiersWithNumber.Any())
            {
                adaugareOptiuniData.SeletedCartier = adaugareOptiuniData.SelectableCartiersWithNumber.First().Key;
            }

            return View(adaugareOptiuniData);
        }

        public ActionResult AdaugaLocalitate()
        {
            string idLocalitateEdit = Request.Form["IdLocalitateEdit"];
            string numeLocalitate = Request.Form["NumeLocalitate"];
            string idJudet = Request.Form["IdJudet"];
            string coordinateGps = Request.Form["CoordinateGps"];
            bool resedintaJudet = (((string)Request.Form["ResedintaJudet"] != null) && (Request.Form["ResedintaJudet"] == "on"));
            bool localitateMica = (((string)Request.Form["LocalitateMica"] != null) && (Request.Form["LocalitateMica"] == "on"));

            //http://stackoverflow.com/questions/547821/two-submit-buttons-in-one-form
            if (Request.Form["SubmitAction"] == "Adauga Localitate Noua")
            {
                var addSuccess = this.unitOfWork.OrasRepository.AddLocalitate(numeLocalitate, idJudet.ParseToInt(), coordinateGps, resedintaJudet, localitateMica);
                this.unitOfWork.Complete();
                if (addSuccess)
                {
                    TempData["Message"] = "Localitatea " + numeLocalitate + " a fost adaugata cu succes";
                }
                else
                {
                    TempData["ErrorMessage"] = "A aparut o eroare la adaugarea localitatii" + numeLocalitate + ", va rugam verificati logurile";
                }

                log.DebugFormat("Localitate {0} added for Judet with id {1}", numeLocalitate, idJudet);
            }
            else if (Request.Form["SubmitAction"] == "Salveaza Modificari")
            {
                log.DebugFormat("Se editeaza localitatea cu id {0} cu numele {1} si coordinate {2}", idLocalitateEdit, numeLocalitate, coordinateGps);
                var editSuccess = this.unitOfWork.OrasRepository.EditeazaLocalitate(Int32.Parse(idLocalitateEdit), coordinateGps, numeLocalitate, resedintaJudet, localitateMica);
                this.unitOfWork.Complete();
                if (editSuccess)
                {
                    TempData["Message"] = "Localitatea " + numeLocalitate + " a fost modificata cu succes";
                }
                else
                {
                    TempData["ErrorMessage"] = "A aparut o eroare la modificarea localitatii " + numeLocalitate + ", va rugam verificati logurile";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "A aparut o eroare, nu s-a specificat daca se face adaugare sau editare";
            }
            return RedirectToAction("Index", "AdaugareOptiuni", new { judetIdParam = idJudet.ParseToInt(), orasIdParam = idLocalitateEdit.ParseToInt() });
        }

        public ActionResult AdaugaCartier()
        {
            string idCartierEdit = Request.Form["IdCartierEdit"];
            string numeCartier = Request.Form["NumeCartier"];
            string idLocalitate = Request.Form["IdLocalitate"];
            string idJudet = Request.Form["IdJudet"];

            if (Request.Form["SubmitAction"] == "Adauga Cartier Nou")
            {
                bool addSuccess = this.unitOfWork.CartierRepository.AddCartier(numeCartier, idLocalitate.ParseToInt());
                this.unitOfWork.Complete();
                if (addSuccess)
                {
                    TempData["Message"] = "Cartierul " + numeCartier + " a fost adaugat cu succes";
                }
                else
                {
                    TempData["Message"] = "A aparut o eroare la adaugarea cartierului " + numeCartier + ", va rugam verificati logurile";
                }
                log.DebugFormat("Cartier {0} added for localitate with id {1}", numeCartier, idLocalitate);
            }
            else if (Request.Form["SubmitAction"] == "Salveaza Modificari")
            {
                log.DebugFormat("Se editeaza cartierul cu id {0} cu numele {1}", idCartierEdit, numeCartier);
                var editSuccess = this.unitOfWork.CartierRepository.EditeazaCartier(Int32.Parse(idCartierEdit), numeCartier);
                this.unitOfWork.Complete();
                if (editSuccess)
                {
                    TempData["Message"] = "Cartierul a fost modificat cu succes";
                }
                else
                {
                    TempData["Message"] = "A aparut o eroare la modificarea cartierului " + numeCartier + ", va rugam verificati logurile";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "A aparut o eroare, nu s-a specificat daca se face adaugare sau editare";
            }
            return RedirectToAction("Index", "AdaugareOptiuni", new { judetIdParam = idJudet.ParseToInt(), orasIdParam = idLocalitate.ParseToInt(), cartierIdParam = idCartierEdit.ParseToInt() });
        }

        public ActionResult DeleteLastLocalitate(int orasID, int judetID)
        {
            var deleteSuccess = this.unitOfWork.OrasRepository.DeleteLocalitate(orasID);
            
            //TODO: Error handling if delete failed?
            this.unitOfWork.Complete();
            if (deleteSuccess)
            {
                TempData["Message"] = "Localitatea "+ orasID + " a fost stearsa cu succes";
            }
            else
            {
                TempData["ErrorMessage"] = "A aparut o eroare la stergerea localitatii, va rugam verificati logurile";
            }

            return RedirectToAction("Index", "AdaugareOptiuni", new { judetIdParam = judetID });
        }

        public ActionResult DeleteLastCartier(int cartierID, int judetID, int localitateID)
        {
            bool deleteSuccess = this.unitOfWork.CartierRepository.DeleteLastCartier(cartierID);
            this.unitOfWork.Complete();

            if (deleteSuccess)
            {
                TempData["Message"] = "Cartierul "+cartierID+" a fost sters cu succes";
            }
            else
            {
                TempData["Message"] = "A aparut o eroare la stergerea cartierului, va rugam verificati logurile";
            }

            return RedirectToAction("Index", "AdaugareOptiuni", new { judetIdParam = judetID, orasIdParam = localitateID});
        }

        [HttpPost]
        public ActionResult UpdateDescriereLocalitate(int localitateID, string localitateDescriere, int judetID)
        {
            bool deleteSuccess = this.unitOfWork.OrasRepository.UpdateDescriptionLocalitate(localitateID, localitateDescriere);
            this.unitOfWork.Complete();
            if (deleteSuccess)
            {
                TempData["Message"] = "Descriere modificata cu succes";
                log.DebugFormat("S-a updatat descrierea pentru localitatea cu id {0}, descriere updatata {1}", localitateID, localitateDescriere);
            }
            else
            {
                TempData["Message"] = "A aparut o eroare la modificarea descrierii, va rugam verificati logurile";
                log.DebugFormat("Eroare la updatat descrierea pentru localitatea cu id {0}, descriere updatata {1}", localitateID, localitateDescriere);
            }

            return RedirectToAction("Index", "AdaugareOptiuni", new { judetIdParam = judetID, orasIdParam = localitateID });
        }
    }
}
