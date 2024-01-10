using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.Managers;
using Imobiliare.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Imobiliare.UI.Models;

namespace Imobiliare.UI.Controllers
{
    [Authorize]
    public class MesajeController : Controller
    {
        private const int DEFAULT_PAGE_SIZE = 12;

        private IUnitOfWork unitOfWork;
        private readonly IEmailManagerService emailManagerService;

        public MesajeController(IUnitOfWork unitOfWork, IEmailManagerService emailManagerService)
        {
            this.unitOfWork = unitOfWork;
            this.emailManagerService = emailManagerService;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ToateMesajele(int? page)
        {
            int selectedPageFinal = 1;
            if (page != null)
            {
                selectedPageFinal = page.Value;
            }

            int totalNumberOfPages;
            List<Mesaj> mesajs = this.unitOfWork.MesajRepository.GetFiltered(x => x.Id > 0, new Entities.Filter { Page = selectedPageFinal, PageSize = DEFAULT_PAGE_SIZE }, out totalNumberOfPages);

            //var mesajs = this.unitOfWork.MesajRepository.Find(x => x.Id > 0).OrderByDescending(x => x.Id);

            MesajeData mesajeData = new MesajeData();
            mesajeData.Mesaje = mesajs;
            mesajeData.Page = selectedPageFinal;
            mesajeData.NumberOfPages = (int)Math.Ceiling((double)totalNumberOfPages / DEFAULT_PAGE_SIZE);

            return View(mesajeData);
        }

        // GET: Mesaje
        public ActionResult Index()
        {
            var mesajs =
              this.unitOfWork.MesajRepository.Find(x => x.ToUser.Email == User.Identity.Name)
                .OrderByDescending(x => x.CreateDateTime);

            var onlyLastMessages = new List<Mesaj>();
            foreach (var mesaj in mesajs)
            {
                if (!onlyLastMessages.Any(x => x.ImobilId == mesaj.ImobilId && x.FromUserId == mesaj.FromUserId && x.ToUserId == mesaj.ToUserId) &&
                  !onlyLastMessages.Any(x => x.FromUserId == mesaj.FromUserId && x.ToUserId == mesaj.ToUserId))
                {
                    onlyLastMessages.Add(mesaj);
                }
            }

            return View(onlyLastMessages.ToList());
        }

        public ActionResult Trimise()
        {
            var mesajs =
              this.unitOfWork.MesajRepository.Find(x => x.FromUser.Email == User.Identity.Name)
                .OrderByDescending(x => x.CreateDateTime);

            var onlyLastMessages = new List<Mesaj>();
            foreach (var mesaj in mesajs)
            {
                if (!onlyLastMessages.Any(x => x.ImobilId == mesaj.ImobilId && x.FromUserId == mesaj.FromUserId && x.ToUserId == mesaj.ToUserId) &&
                  !onlyLastMessages.Any(x => x.FromUserId == mesaj.FromUserId && x.ToUserId == mesaj.ToUserId))
                {
                    onlyLastMessages.Add(mesaj);
                }
            }

            return View(onlyLastMessages.ToList());
        }

        // GET: Mesaje/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Mesaj mesaj = this.unitOfWork.MesajRepository.SingleOrDefault(x => x.Id == id.Value);
            if (mesaj == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            if (mesaj.CitireDateTime == DateTime.MinValue)
            {
                mesaj.CitireDateTime = DateTime.Now;
            }
            this.unitOfWork.Complete();

            List<Mesaj> conversation =
              this.unitOfWork.MesajRepository.Find(
                x =>
                  (x.Imobil != null && x.ImobilId == mesaj.ImobilId) &&
                  ((x.FromUserId == mesaj.FromUserId && x.ToUserId == mesaj.ToUserId) ||
                   (x.FromUserId == mesaj.ToUserId && x.ToUserId == mesaj.FromUserId))
              );

            var mesajConversationModel = new MesajConversationModel();
            mesajConversationModel.Mesaje = conversation;
            mesajConversationModel.MesajSelectat = mesaj;
            //mesajConversationModel.Imobil = this.unitOfWork.AnunturiRepository.Single(x => x.Id == mesaj.ImobilId);

            if (mesaj.FromUser == null)
            {
                mesaj.FromUser = new UserProfile
                {
                    Email = "userDeleted@u.com"
                };
            }

            if (mesaj.FromUser.Email == User.Identity.Name)
            {
                mesajConversationModel.LoggedInUserEmail = mesaj.FromUser.Email;
                mesajConversationModel.ToUserEmail = mesaj.ToUser.Email;
            }
            else
            {
                mesajConversationModel.LoggedInUserEmail = mesaj.ToUser.Email;
                mesajConversationModel.ToUserEmail = mesaj.FromUser.Email;
            }

            return View(mesajConversationModel);
        }

        // GET: Mesaje/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Mesaje/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Mesaj mesaj = this.unitOfWork.MesajRepository.SingleOrDefault(x => x.Id == id.Value);
            if (mesaj == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(mesaj);
        }

        // POST: Mesaje/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mesaj mesaj = this.unitOfWork.MesajRepository.SingleOrDefault(x => x.Id == id);
            this.unitOfWork.MesajRepository.Delete(mesaj);
            this.unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult TrimiteMesajVanzatoruluiAnuntRelated(string mesajCumparator, string idAnuntVanzator)
        {
            //TODO: Check if not null
            var fromUser = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);

            var intIdAnunt = Int32.Parse(idAnuntVanzator);
            var toAnunt = this.unitOfWork.AnunturiRepository.Single(x => x.Id == intIdAnunt);
            var toUser = this.unitOfWork.UsersRepository.Single(x => x.Email == toAnunt.UserProfile.Email);

            if (string.IsNullOrEmpty(mesajCumparator))
            {
                var msg = "Nu puteți trimite un mesaj gol!";
                this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Warning,
                  $"Userul {fromUser.Email} incearca sa trimita un mesaj gol");
                this.unitOfWork.Complete();
                return Content(msg);
            }

            if (fromUser.Email == toUser.Email)
            {
                var msg = "Nu puteți trimite mesaj la anunțul propriu!";
                this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Warning,
                  $"Userul {fromUser.Email} incearca sa isi trimita singur email, se restrictioneaza mesajul {mesajCumparator}", notifyAdmin: true);
                this.unitOfWork.Complete();
                return Content(msg);
            }

            this.unitOfWork.MesajRepository.Add(new Mesaj()
            {
                FromUser = fromUser,
                ToUser = toUser,
                Imobil = toAnunt,
                Continut = mesajCumparator,
                Categorie = MesajCategorie.Anunt
            });
            this.unitOfWork.Complete();

            //Send email to address
            this.emailManagerService.SendAnuntCereDetaliiEmail(toUser.Email, toAnunt.Id, toAnunt.Title);

            var mesaj = "Un email a fost trimis la adresa de email a vanzatorului cu mesajul dumneavoastra.";
            this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message,
              $"TrimiteMesajVanzatoruluiAnuntRelated de la {fromUser.Email} la {toUser.Email}, mesajul {mesajCumparator}", notifyAdmin: true);
            this.unitOfWork.Complete();

            return Content(mesaj);
        }

        public ActionResult RaspundeMesajAnuntRelated(string mesajRaspuns, int mesajId, string mesajLoggedInUserEmail, string mesajToUserEmail)
        {
            var anunt = this.unitOfWork.MesajRepository.Single(x => x.Id == mesajId).Imobil;

            this.unitOfWork.MesajRepository.Add(new Mesaj()
            {
                FromUser = this.unitOfWork.UsersRepository.Single(x => x.Email == mesajLoggedInUserEmail),
                ToUser = this.unitOfWork.UsersRepository.Single(x => x.Email == mesajToUserEmail),
                Imobil = anunt,
                Continut = mesajRaspuns,
                Categorie = MesajCategorie.Anunt
            });
            this.unitOfWork.Complete();

            this.emailManagerService.SendAnuntCereDetaliiEmail(mesajToUserEmail, anunt.Id, anunt.Title);

            this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message,
              $"TrimiteMesajVanzatorului Raspuns de la {mesajLoggedInUserEmail} la {mesajToUserEmail}, mesajul {mesajRaspuns}", notifyAdmin: true);
            this.unitOfWork.Complete();

            TempData["Message"] = "Mesaj trimis cu succes";
            return RedirectToAction("Details", new { id = mesajId });
        }
    }
}
