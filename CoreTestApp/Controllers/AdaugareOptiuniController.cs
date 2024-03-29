﻿using System;
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



        public ActionResult Index(int? JudetSelect, int? OrasSelect, int? CartierSelect)
        {
            var judetId = JudetSelect != null ? JudetSelect.Value : 0;
            var orasId = OrasSelect != null ? OrasSelect.Value : 0;
            var cartierId = CartierSelect != null ? CartierSelect.Value : 0;

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

        public ActionResult AdaugaLocalitate(string IdLocalitateEdit, string NumeLocalitate, string IdJudet, string CoordinateGps, string ResedintaJudet, string LocalitateMica)
        {
            bool resedintaJudet = (ResedintaJudet != null) && (ResedintaJudet == "on");
            bool localitateMica = (LocalitateMica != null) && (LocalitateMica == "on");

            //http://stackoverflow.com/questions/547821/two-submit-buttons-in-one-form
            if (Request.Form["SubmitAction"] == "Adauga Localitate Noua")
            {
                var addSuccess = this.unitOfWork.OrasRepository.AddLocalitate(NumeLocalitate, IdJudet.ParseToInt(), CoordinateGps, resedintaJudet, localitateMica);
                this.unitOfWork.Complete();
                if (addSuccess)
                {
                    TempData["Message"] = "Localitatea " + NumeLocalitate + " a fost adaugata cu succes";
                }
                else
                {
                    TempData["ErrorMessage"] = "A aparut o eroare la adaugarea localitatii" + NumeLocalitate + ", va rugam verificati logurile";
                }

                log.Debug($"Localitate {NumeLocalitate} added for Judet with id {IdJudet}");
            }
            else if (Request.Form["SubmitAction"] == "Salveaza Modificari")
            {
                log.Debug($"Se editeaza localitatea cu id {IdLocalitateEdit} cu numele {NumeLocalitate} si coordinate {CoordinateGps}");
                var editSuccess = this.unitOfWork.OrasRepository.EditeazaLocalitate(Int32.Parse(IdLocalitateEdit), CoordinateGps, NumeLocalitate, resedintaJudet, localitateMica);
                this.unitOfWork.Complete();
                if (editSuccess)
                {
                    TempData["Message"] = "Localitatea " + NumeLocalitate + " a fost modificata cu succes";
                }
                else
                {
                    TempData["ErrorMessage"] = "A aparut o eroare la modificarea localitatii " + NumeLocalitate + ", va rugam verificati logurile";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "A aparut o eroare, nu s-a specificat daca se face adaugare sau editare";
            }
            return RedirectToAction("Index", "AdaugareOptiuni", new { JudetSelect = IdJudet.ParseToInt(), OrasSelect = IdLocalitateEdit.ParseToInt() });
        }

        public ActionResult AdaugaCartier(string IdCartierEdit, string NumeCartier, string IdLocalitate, string IdJudet)
        {
            if (Request.Form["SubmitAction"] == "Adauga Cartier Nou")
            {
                bool addSuccess = this.unitOfWork.CartierRepository.AddCartier(NumeCartier, IdLocalitate.ParseToInt());
                this.unitOfWork.Complete();
                if (addSuccess)
                {
                    TempData["Message"] = "Cartierul " + NumeCartier + " a fost adaugat cu succes";
                }
                else
                {
                    TempData["Message"] = "A aparut o eroare la adaugarea cartierului " + NumeCartier + ", va rugam verificati logurile";
                }
                log.Debug($"Cartier {NumeCartier} added for localitate with id {IdLocalitate}");
            }
            else if (Request.Form["SubmitAction"] == "Salveaza Modificari")
            {
                log.Debug($"Se editeaza cartierul cu id {IdCartierEdit} cu numele {NumeCartier}");
                var editSuccess = this.unitOfWork.CartierRepository.EditeazaCartier(Int32.Parse(IdCartierEdit), NumeCartier);
                this.unitOfWork.Complete();
                if (editSuccess)
                {
                    TempData["Message"] = "Cartierul a fost modificat cu succes";
                }
                else
                {
                    TempData["Message"] = "A aparut o eroare la modificarea cartierului " + NumeCartier + ", va rugam verificati logurile";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "A aparut o eroare, nu s-a specificat daca se face adaugare sau editare";
            }
            return RedirectToAction("Index", "AdaugareOptiuni", new { JudetSelect = IdJudet.ParseToInt(), OrasSelect = IdLocalitate.ParseToInt(), CartierSelect = IdCartierEdit.ParseToInt() });
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

            return RedirectToAction("Index", "AdaugareOptiuni", new { JudetSelect = judetID });
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

            return RedirectToAction("Index", "AdaugareOptiuni", new { JudetSelect = judetID, OrasSelect = localitateID});
        }

        [HttpPost]
        public ActionResult UpdateDescriereLocalitate(int localitateID, string localitateDescriere, int judetID)
        {
            bool deleteSuccess = this.unitOfWork.OrasRepository.UpdateDescriptionLocalitate(localitateID, localitateDescriere);
            this.unitOfWork.Complete();
            if (deleteSuccess)
            {
                TempData["Message"] = "Descriere modificata cu succes";
                log.Debug($"S-a updatat descrierea pentru localitatea cu id {localitateID}, descriere updatata {localitateDescriere}");
            }
            else
            {
                TempData["Message"] = "A aparut o eroare la modificarea descrierii, va rugam verificati logurile";
                log.Debug($"Eroare la updatat descrierea pentru localitatea cu id {localitateID}, descriere updatata {localitateDescriere}");
            }

            return RedirectToAction("Index", "AdaugareOptiuni", new { JudetSelect = judetID, OrasSelect = localitateID });
        }
    }
}
