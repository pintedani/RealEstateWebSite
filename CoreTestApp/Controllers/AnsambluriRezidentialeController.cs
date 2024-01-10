using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.UI.Models;
using Logging;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Imobiliare.UI.Controllers
{
    public class AnsambluriRezidentialeController : Controller
    {
        private const int DEFAULT_PAGE_SIZE = 12;

        private static readonly ILog log = LogManager.GetLogger(typeof(AnsambluriRezidentialeController));

        private IUnitOfWork unitOfWork;

        public AnsambluriRezidentialeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: AnsambluriRezidentiale
        public ActionResult Index(int? page)
        {
            var ansambluriViewModel = CreateAnsambluriViewModel(page, true);
            return View(ansambluriViewModel);
        }

        private AnsambluriViewModel CreateAnsambluriViewModel(int? page, bool? active)
        {
            string localitateId = null;
            if (Request.HasFormContentType)
            {
                localitateId = Request.Form["OrasSelect"];
            }

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
            List<Ansamblu> ansambluri = this.unitOfWork.AnsambluriRepository.GetAnsambluri(
                new AnsambluriRezidentialeFilter()
                { Active = active, OrasId = orasId, Page = selectedPageFinal, PageSize = DEFAULT_PAGE_SIZE },
                out totalNumberOfPages);

            var displayPageNumber = (int)Math.Ceiling((double)totalNumberOfPages / DEFAULT_PAGE_SIZE);
            var ansambluriViewModel = new AnsambluriViewModel();
            ansambluriViewModel.Ansambluri = ansambluri.OrderByDescending(x => x.DateCreated).ToList();
            ansambluriViewModel.Oras = oras;
            ansambluriViewModel.TotalNumberOfEntries = totalNumberOfPages;
            ansambluriViewModel.NumberOfPages = displayPageNumber;
            ansambluriViewModel.Page = selectedPageFinal;
            return ansambluriViewModel;
        }

        // GET: AgentiiImobiliare
        public ActionResult ToateAnsamblurileAdmin(int? page)
        {
            var ansambluriViewModel = CreateAnsambluriViewModel(page, null);

            return View(ansambluriViewModel);
        }

        public ActionResult AnsambluRezidential(int? ansambluId)
        {
            if (ansambluId == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Ansamblu ansamblu = this.unitOfWork.AnsambluriRepository.Single(x => x.Id == ansambluId.Value);
            if (ansamblu == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            AnsambluRezidentialViewModel ansambluRezidentialViewModel = new AnsambluRezidentialViewModel();
            ansambluRezidentialViewModel.Ansamblu = ansamblu;

            var name = string.Empty;
            if (User.Identity.Name != string.Empty)
            {
                name = User.Identity.Name;
            }

            //if (!Request.Iscrawler())
            //{
            //    log.Debug($"User {0} accessed ansamblu rezidential with id {1}", name != string.Empty ? name : "unknown user", ansambluId.Value);
            //    //if(!apartamentDetaliiData.IsCurrentUserAdmin)
            //    this.unitOfWork.AnsambluriRepository.IncrementNumarAccesari(ansambluId.Value);
            //    this.unitOfWork.Complete();
            //}

            return View(ansambluRezidentialViewModel);
        }

        // GET: AnsambluriRezidentiale/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AnsambluriRezidentiale/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Titlu,Continut,NumarVizualizari,Poze,Active,Keywords,OrasSelect,GoogleMarkerCoordinateLat,GoogleMarkerCoordinateLong")] Ansamblu ansamblu)
        {
            if (ModelState.IsValid)
            {
                ansamblu.DateCreated = DateTime.Now;
                ansamblu.DateModified = DateTime.Now;
                ansamblu.Oras = this.unitOfWork.OrasRepository.Single(x => x.Id == ansamblu.OrasSelect);
                ansamblu.UserProfile = this.unitOfWork.UsersRepository.Single(x => x.UserName == User.Identity.Name);
                this.unitOfWork.AnsambluriRepository.Add(ansamblu);
                this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"Created ansamblu: {ansamblu.Titlu}", userName: User.Identity.Name, notifyAdmin: true);
                this.unitOfWork.Complete();
                TempData["FooterStatus"] = "Ansamblu adaugat cu succes";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Eroare la crearea ansamblului, va rugam completati toate campurile";
            return View(ansamblu);
        }

        // GET: AnsambluriRezidentiale/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Ansamblu ansamblu = this.unitOfWork.AnsambluriRepository.Single(x => x.Id == id.Value);
            if (ansamblu == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            if (ansamblu.OrasSelect != 0)
            {
                ViewBag.OrasSelectat = this.unitOfWork.OrasRepository.Single(x => x.Id == ansamblu.OrasSelect);
            }
            return View(ansamblu);
        }

        // POST: AnsambluriRezidentiale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Titlu,Continut,DateCreated,DateModified,UserId,NumarVizualizari,Poze,Active,Keywords,OrasSelect,GoogleMarkerCoordinateLat,GoogleMarkerCoordinateLong")] Ansamblu ansamblu)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.AnsambluriRepository.Edit(ansamblu);
                this.unitOfWork.Complete();
                log.Debug($"Edited ansamblu rezidential: {ansamblu.Titlu}");
                TempData["FooterStatus"] = "Ansamblu rezidential editat cu succes";
                return RedirectToAction("Index");
            }
            return View(ansamblu);
        }

        // GET: AnsambluriRezidentiale/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Ansamblu ansamblu = this.unitOfWork.AnsambluriRepository.Single(x => x.Id == id.Value);
            if (ansamblu == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(ansamblu);
        }

        // POST: AnsambluriRezidentiale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.unitOfWork.AnsambluriRepository.Delete(id);
            this.unitOfWork.Complete();
            log.Error($"Deleted ansamblu rezidential with id: {id}");
            TempData["FooterStatus"] = "Ansamblu rezidential sters cu succes";
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public ActionResult AddImageNoAjax(HttpPostedFileBase file)
        //{
        //    int ansambluId = int.Parse(Request.Form["ansambluId"]);
        //    if (file != null)
        //    {
        //        var imageName = this.unitOfWork.AnsambluriRepository.AddImage(ansambluId, new[] { file });
        //        this.unitOfWork.Complete();
        //        log.Debug($"Added images for ansamblu NOT async with id {0} by user {1}, imageName: {2}", ansambluId, User.Identity.Name, imageName);
        //    }
        //    else
        //    {
        //        log.Debug($"No image selected for ansamblu NOT async  by user {0}", User.Identity.Name);
        //    }
        //    return RedirectToAction("Edit", new { id = ansambluId });
        //}

        public ActionResult StergeImagine(int ansambluId, string poza)
        {
            if (ansambluId == 0 || string.IsNullOrEmpty(poza))
            {
                TempData["ErrorMessage"] = "Id stire or poza empty";
                return RedirectToAction("Edit", new { id = ansambluId });
            }

            this.unitOfWork.AnsambluriRepository.DeleteImage(ansambluId, poza);
            this.unitOfWork.Complete();
            TempData["Message"] = "Imagine stearsa cu succes";
            return RedirectToAction("Edit", new { id = ansambluId });
        }
    }
}
