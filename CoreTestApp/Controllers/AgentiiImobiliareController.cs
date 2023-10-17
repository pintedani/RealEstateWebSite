using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.UI.Models;
using Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Imobiliare.UI.Controllers
{
    public class AgentiiImobiliareController : Controller
    {
        private const int DEFAULT_PAGE_SIZE = 12;
        private static readonly ILog log = LogManager.GetLogger(typeof(AgentiiImobiliareController));

        private IUnitOfWork unitOfWork;

        public AgentiiImobiliareController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: AgentiiImobiliare
        public ActionResult Index(int? page)
        {
            var agentiiViewModel = CreateAgentiiViewModel(page);

            return View(agentiiViewModel);
        }

        public ActionResult IndexB5(int? page)
        {
            var agentiiViewModel = CreateAgentiiViewModel(page);

            return View(agentiiViewModel);
        }

        private AgentiiViewModel CreateAgentiiViewModel(int? page)
        {
            var localitateId = Request.Form["OrasSelect"];

            Oras oras = null;
            int orasId = 0;
            if (!string.IsNullOrEmpty(localitateId))
            {
                var parsedId = int.Parse(localitateId);
                if (parsedId != 0)
                {
                    oras = this.unitOfWork.OrasRepository.Single(x => x.Id == parsedId);
                    orasId = oras.Id;
                }
            }

            int selectedPageFinal = 1;
            if (page != null)
            {
                selectedPageFinal = page.Value;
            }

            int totalNumberOfPages;
            List<Agentie> agenties = this.unitOfWork.AgentiiRepository.GetAgenties(
                new AgentiiImobiliareFilter
                { OrasId = orasId, Page = selectedPageFinal, PageSize = DEFAULT_PAGE_SIZE },
                out totalNumberOfPages);

            var displayPageNumber = (int)Math.Ceiling((double)totalNumberOfPages / DEFAULT_PAGE_SIZE);
            var agentiiViewModel = new AgentiiViewModel();
            agentiiViewModel.Agentii = agenties;
            agentiiViewModel.Oras = oras;
            agentiiViewModel.TotalNumberOfEntries = totalNumberOfPages;
            agentiiViewModel.NumberOfPages = displayPageNumber;
            agentiiViewModel.Page = selectedPageFinal;
            return agentiiViewModel;
        }

        // GET: AgentiiImobiliare
        [Authorize(Roles = "Admin")]
        public ActionResult ToateAgentiileAdmin(int? page)
        {
            var agentiiViewModel = CreateAgentiiViewModel(page);

            return View(agentiiViewModel);
        }

        // GET: AgentiiImobiliare/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Agentie agentie = this.unitOfWork.AgentiiRepository.Single(x => x.Id == id.Value);
            if (agentie == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound); ;
            }
            return View(agentie);
        }

        public ActionResult Lista(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            var agenties = this.unitOfWork.AgentiiRepository.GetAgentiiForOras(id.Value, false);

            return View(agenties);
        }

        public ActionResult Detalii(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Agentie agentie = this.unitOfWork.AgentiiRepository.Single(x => x.Id == id.Value);
            if (agentie == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(agentie);
        }


        // GET: AgentiiImobiliare/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Agentie agentie = this.unitOfWork.AgentiiRepository.Single(x => x.Id == id.Value);
            if (agentie == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            if (agentie.OrasSelect != null && agentie.OrasSelect != 0)
            {
                ViewBag.OrasSelectat = this.unitOfWork.OrasRepository.Single(x => x.Id == agentie.OrasSelect);
            }
            return View(agentie);
        }

        //// POST: AgentiiImobiliare/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Nume,OrasSelect,DescriereAgentie,Website,PozaAgentie,TelefonAgentie,AfisarePrimaPagina")] Agentie agentie)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.AgentiiRepository.Update(agentie);
                this.unitOfWork.Complete();
                return RedirectToAction("ToateAgentiileAdmin");
            }
            //ViewBag.OrasId = new SelectList(db.Orase, "Id", "Nume", agentie.OrasId);
            return View(agentie);
        }

        //GET: AgentiiImobiliare/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Agentie agentie = this.unitOfWork.AgentiiRepository.Single(x => x.Id == id.Value);
            if (agentie == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            this.unitOfWork.AgentiiRepository.Delete(agentie);
            this.unitOfWork.Complete();

            TempData["Message"] = $"Agentia cu nume {agentie.Nume} si id {id} a fost stearsa";
            log.DebugFormat($"Admin deleted agentie {agentie.Nume} with id {id}");

            return this.RedirectToAction("ToateAgentiileAdmin");
        }
    }
}
