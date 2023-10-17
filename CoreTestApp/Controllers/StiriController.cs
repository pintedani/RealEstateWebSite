using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.UI.Models;
using Imobiliare.UI.Utils.UrlSeoFormatters;
using Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Imobiliare.UI.Controllers
{
    public class StiriController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        private static readonly ILog log = LogManager.GetLogger(typeof(StiriController));

        public StiriController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Stiri
        public ActionResult Index(int? page)
        {
            var selectedPageFinal = 1;

            if (page != null)
            {
                selectedPageFinal = page.Value;
            }

            var stiriViewModel = CreateStiriViewModel(selectedPageFinal, true);
            return View(stiriViewModel);
        }

        private StiriViewModel CreateStiriViewModel(int selectedPageFinal, bool? onlyActive)
        {
            StiriFilter stiriFilter = new StiriFilter();
            stiriFilter.Active = onlyActive;
            stiriFilter.Page = selectedPageFinal;
            StiriViewModel stiriViewModel = new StiriViewModel();
            int totalNumberOfStiri;
            stiriViewModel.Stiri = this.unitOfWork.StiriRepository.GetFiltered(
                x => stiriFilter.Active == null || stiriFilter.Active != null && x.Active == stiriFilter.Active.Value,
                stiriFilter, out totalNumberOfStiri);
            stiriViewModel.StiriFilter = stiriFilter;
            var displayPageNumber = (int)Math.Ceiling((double)totalNumberOfStiri / stiriFilter.PageSize);
            stiriViewModel.NumberOfPages = displayPageNumber;
            return stiriViewModel;
        }

        [Authorize(Roles = "Admin")]
        // GET: Stiri
        public ActionResult IndexAdmin(int? page)
        {
            var selectedPageFinal = 1;

            if (page != null)
            {
                selectedPageFinal = page.Value;
            }

            var stiriViewModel = CreateStiriViewModel(selectedPageFinal, null);
            return View(stiriViewModel);
        }

        [Authorize(Roles = "Admin")]
        // GET: Stiri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            //Use cached single overriden version
            Stire stire = this.unitOfWork.StiriRepository.Single(x => x.Id == id.Value);
            if (stire == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(stire);
        }

        [Authorize(Roles = "Admin")]
        // GET: Stiri/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stiri/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
          [Bind("StireId,CategorieStire,Titlu,TitluUrl,Continut,DateCreated,DateModified,NumarVizualizari,Poze,Active,AfiseazaPrimaPagina,Keywords,OrasSelect,MetaDescription"
        )] Stire stire)
        {
            if (ModelState.IsValid)
            {
                stire.UserProfile = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);
                stire.DateCreated = DateTime.Now;
                stire.DateModified = DateTime.Now;
                this.unitOfWork.StiriRepository.Add(stire);
                this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"Created stire: {stire.Titlu}", userName: User.Identity.Name, notifyAdmin: true);
                this.unitOfWork.Complete();
                TempData["FooterStatus"] = "Stire creata cu succes";
                return RedirectToAction("Edit", new { id = stire.Id });
            }

            TempData["ErrorMessage"] = "Eroare la crearea stiri, va rugam completati toate campurile";
            return View(stire);
        }

        [Authorize(Roles = "Admin")]
        // GET: Stiri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Stire stire = this.unitOfWork.StiriRepository.Single(x => x.Id == id.Value);
            if (stire == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            if (stire.OrasSelect != 0)
            {
                ViewBag.OrasSelectat = this.unitOfWork.OrasRepository.Single(x => x.Id == stire.OrasSelect);
            }
            return View(stire);
        }

        [Authorize(Roles = "Admin")]
        // POST: Stiri/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
          [Bind("Id,CategorieStire,Titlu,TitluUrl,Continut,DateCreated,DateModified,NumarVizualizari,Poze,Active,AfiseazaPrimaPagina,Keywords,OrasSelect,MetaDescription"
        )] Stire stire)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.StiriRepository.Edit(stire);
                this.unitOfWork.Complete();
                log.DebugFormat("Edited stire: {0}", stire.Titlu);
                TempData["FooterStatus"] = "Stire editata cu succes";
                TempData["Message"] = "Stire editata cu succes";
                return RedirectToAction("Edit", new { id = stire.Id });
            }
            return View(stire);
        }

        [Authorize(Roles = "Admin")]
        // GET: Stiri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Stire stire = this.unitOfWork.StiriRepository.Single(x => x.Id == id.Value);
            if (stire == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(stire);
        }

        [Authorize(Roles = "Admin")]
        // POST: Stiri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.unitOfWork.StiriRepository.Delete(id);
            this.unitOfWork.Complete();
            log.DebugFormat("Deleted stire with id: {0}", id.ToString());
            TempData["FooterStatus"] = "Stire stearsa cu succes";
            return RedirectToAction("Index");
        }

        [Route("Stire/{titlu}/{stireId}")]
        public ActionResult Stire(string titlu, string stireId)
        {
            int id;
            int.TryParse(stireId, out id);
            if (id == 0)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            var stire = this.unitOfWork.StiriRepository.TryGetCachedStire(id);

            //Check if title changed in the meantime, do 301 redirect in case
            //Same in HomeController - ApartamentDetalii
            var url = StiriUrls.BuildStirePageUrl(stire);

            var newTitle = url.Split('/')[2];

            if (!string.IsNullOrEmpty(titlu) && titlu != newTitle)
            {
                log.WarnFormat("STIRE Redirect permanent because of stire title change for stire {0} with original title {1} and updated title {2}", stire.Id, titlu, newTitle);
                return RedirectPermanent(url);
            }

            //Log
            var name = string.Empty;
            if (User.Identity.Name != string.Empty)
            {
                name = User.Identity.Name;
            }
            //if (!Request.Iscrawler())
            //{
            //    log.DebugFormat("User: {0}, ip: {1} accessed stire with id {2}, titlu {3}", name != string.Empty ? name : "unknown user",
            //      Request.UserHostAddress, stire.Id, titlu);
            //}

            var stireViewModel = new StireViewModel();
            stireViewModel.Stire = stire;

            if (stire.OrasSelect != 0)
            {
                var imobilFilter = new ImobilFilter
                {
                    PageSize = 4,
                    OrasId = stire.OrasSelect,
                    StareAprobare = StareAprobare.Aprobat
                };
                int totalNumberOfPages;
                var anunturi = this.unitOfWork.AnunturiRepository.GetAllImobil(imobilFilter, out totalNumberOfPages);
                if (anunturi.Count != 0)
                {
                    stireViewModel.LastAddedImobils = anunturi;
                }
                else
                {
                    stireViewModel.LastAddedImobils = this.unitOfWork.AnunturiRepository.GetLastAddedImobils(4);
                }
            }
            else
            {
                stireViewModel.LastAddedImobils = this.unitOfWork.AnunturiRepository.GetLastAddedImobils(4);
            }
            StiriFilter stiriFilter = new StiriFilter();
            stiriFilter.Active = true;
            Tuple<Stire, Stire> stiriNavigation = this.unitOfWork.StiriRepository.FindBeforeAndAfterNavigationStiri(id, stiriFilter);

            stireViewModel.StireAnterioara = stiriNavigation.Item1;
            stireViewModel.StireUrmatoare = stiriNavigation.Item2;

            return this.View("Stire", stireViewModel);
        }

        //[HttpPost]
        //public ActionResult AddImageNoAjax(HttpPostedFileBase file)
        //{
        //    int stireId = int.Parse(Request.Form["stireId"]);

        //    if (file != null)
        //    {
        //        //Max 1 MB Photos
        //        if ((file.ContentLength / 1048576.0) > 1)
        //        {
        //            TempData["ErrorMessage"] = "Imaginea depaseste 1MB! Redimensionati imaginea si incercati din nou.";
        //            return RedirectToAction("Edit", new { id = stireId });
        //        }

        //        var imageName = this.unitOfWork.StiriRepository.AddImage(stireId, new[] { file });
        //        this.unitOfWork.Complete();
        //        log.DebugFormat("Added images for stire NOT async with id {0} by user {1}, imageName: {2}", stireId, User.Identity.Name, imageName);
        //    }
        //    else
        //    {
        //        log.DebugFormat("No image selected for stire NOT async  by user {0}", User.Identity.Name);
        //    }
        //    return RedirectToAction("Edit", new { id = stireId });
        //}

        public ActionResult StergeImagine()
        {
            int stireId = int.Parse(Request.Form["stireId"]);
            string poza = Request.Form["poza"];

            if (stireId == 0 || string.IsNullOrEmpty(poza))
            {
                TempData["ErrorMessage"] = "Id stire or poza empty";
                return RedirectToAction("Edit", new { id = stireId });
            }

            this.unitOfWork.StiriRepository.DeleteImage(stireId, poza);
            this.unitOfWork.Complete();
            TempData["Message"] = "Imagine stearsa cu succes";
            return RedirectToAction("Edit", new { id = stireId });
        }
    }
}
