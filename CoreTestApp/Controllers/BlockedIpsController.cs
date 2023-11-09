//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using Imobiliare.Entities;
//using Imobiliare.Repositories;
//using Microsoft.AspNetCore.Mvc;

//namespace Imobiliare.UI.Controllers
//{
//    public class BlockedIpsController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: BlockedIps
//        public ActionResult Index()
//        {
//            return View(db.BlockedIps.ToList());
//        }

//        // GET: BlockedIps/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return StatusCode((int)HttpStatusCode.BadRequest);
//            }
//            BlockedIp blockedIp = db.BlockedIps.Find(id);
//            if (blockedIp == null)
//            {
//                return StatusCode((int)HttpStatusCode.NotFound);
//            }
//            return View(blockedIp);
//        }

//        // GET: BlockedIps/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: BlockedIps/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind("Address,Descriere,TraceBlocking,UpdateBlockCount,IsRegex,Enabled")] BlockedIp blockedIp)
//        {
//            if (ModelState.IsValid)
//            {
//                db.BlockedIps.Add(blockedIp);
//                blockedIp.DataAdaugare = DateTime.Now;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(blockedIp);
//        }

//        // GET: BlockedIps/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return StatusCode((int)HttpStatusCode.BadRequest);
//            }
//            BlockedIp blockedIp = db.BlockedIps.Find(id);
//            if (blockedIp == null)
//            {
//                return StatusCode((int)HttpStatusCode.NotFound);
//            }
//            return View(blockedIp);
//        }

//        // POST: BlockedIps/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind("Id,Address,BlockCount,Descriere,TraceBlocking,DataAdaugare,UpdateBlockCount,IsRegex,Enabled")] BlockedIp blockedIp)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(blockedIp).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(blockedIp);
//        }

//        // GET: BlockedIps/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return StatusCode((int)HttpStatusCode.BadRequest);
//            }
//            BlockedIp blockedIp = db.BlockedIps.Find(id);
//            if (blockedIp == null)
//            {
//                return StatusCode((int)HttpStatusCode.NotFound);
//            }
//            return View(blockedIp);
//        }

//        // POST: BlockedIps/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            BlockedIp blockedIp = db.BlockedIps.Find(id);
//            db.BlockedIps.Remove(blockedIp);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
