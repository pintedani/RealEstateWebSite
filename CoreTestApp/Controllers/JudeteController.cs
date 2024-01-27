using System.Net;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliare.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class JudeteController : Controller
    {
        private IUnitOfWork unitOfWork;

        public JudeteController(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Judet
        public ActionResult Index()
        {
            return View(this.unitOfWork.JudetRepository.Judete());
        }

        // GET: Judet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            Judet judet = this.unitOfWork.JudetRepository.Single(x => x.Id == id);
            if (judet == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(judet);
        }

        // GET: Judet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Judet judet = this.unitOfWork.JudetRepository.Single(x => x.Id == id);
            if (judet == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(judet);
        }

        // POST: Judet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind("Id,Nume,PrescurtareAuto,CoordinateGps,Descriere")] Judet judet)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.JudetRepository.Edit(judet);
                this.unitOfWork.Complete();
                TempData["Message"] = "Judet " + judet.Nume + " editat cu succes";

                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Date invalide!";
            return View(judet);
        }
    }
}
