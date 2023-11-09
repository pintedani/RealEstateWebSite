using System;
using System.Linq;
using System.Net;
using Imobiliare.Entities;
using Imobiliare.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliare.UI.Controllers
{
  [Authorize(Roles = "Admin")]
  public class EmailTemplateController : Controller
  {
    //private ApplicationDbContext db = new ApplicationDbContext();

    private IEmailManagerService emailManagerService;

    public EmailTemplateController(IEmailManagerService emailManagerService)
    {
      this.emailManagerService = emailManagerService;
    }

    // GET: EmailTemplate
    public ActionResult Index()
    {
      return View(emailManagerService.GetAllEmailTemplates().OrderBy(x => x.EmailTemplateCategory));
    }

    // GET: EmailTemplate/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return StatusCode((int)HttpStatusCode.BadRequest);
      }
      EmailTemplate emailTemplate = emailManagerService.GetEmailTemplate(id.Value);
      if (emailTemplate == null)
      {
        return StatusCode((int)HttpStatusCode.NotFound);
      }
      ViewBag.EmailFooter =
        emailManagerService.GetAllEmailTemplates()
          .First(x => x.EmailTemplateCategory == EmailTemplateCategory.EmailFooter);
      return View(emailTemplate);
    }

    // GET: EmailTemplate/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: EmailTemplate/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(
      [Bind("Id,HumanReadableEmailIdentifier,Titlu,Mesaj,DateCreated,DateModified,EmailTemplateCategory")] EmailTemplate emailTemplate)
    {
      if (ModelState.IsValid)
      {
        emailTemplate.DateCreated = DateTime.Now;
        emailTemplate.DateModified = DateTime.Now;
        emailManagerService.AddEmailTemplate(emailTemplate);
        return RedirectToAction("Index");
      }

      return View(emailTemplate);
    }

    // GET: EmailTemplate/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return StatusCode((int)HttpStatusCode.BadRequest);
      }
      EmailTemplate emailTemplate = emailManagerService.GetEmailTemplate(id.Value);
      if (emailTemplate == null)
      {
        return StatusCode((int)HttpStatusCode.NotFound);
      }
      ViewBag.EmailFooter =
        emailManagerService.GetAllEmailTemplates()
          .First(x => x.EmailTemplateCategory == EmailTemplateCategory.EmailFooter);
      return View(emailTemplate);
    }

    // POST: EmailTemplate/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(
      [Bind("Id,HumanReadableEmailIdentifier,Titlu,Mesaj,DateCreated,DateModified,EmailTemplateCategory")] EmailTemplate emailTemplate)
    {
      if (ModelState.IsValid)
      {
        emailManagerService.UpdateEmailTemplate(emailTemplate);
        return RedirectToAction("Index");
      }
      return View(emailTemplate);
    }

    // GET: EmailTemplate/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return StatusCode((int)HttpStatusCode.BadRequest);
      }
      EmailTemplate emailTemplate = emailManagerService.GetEmailTemplate(id.Value);
      if (emailTemplate == null)
      {
        return StatusCode((int)HttpStatusCode.NotFound);
      }
      return View(emailTemplate);
    }

    // POST: EmailTemplate/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      emailManagerService.DeleteEmailTemplate(id);
      return RedirectToAction("Index");
    }
  }
}