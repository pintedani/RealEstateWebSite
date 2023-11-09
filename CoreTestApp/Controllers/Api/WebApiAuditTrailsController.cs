//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Description;
//using Imobiliare.Entities;
//using Imobiliare.Repositories;

//namespace Imobiliare.UI.Controllers
//{
//    public class WebApiAuditTrailsController : ApiController
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: api/AuditTrails
//        public IQueryable<AuditTrail> GetAuditTrail()
//        {
//            return db.AuditTrail;
//        }

//        // GET: api/AuditTrails/5
//        [ResponseType(typeof(AuditTrail))]
//        public async Task<IHttpActionResult> GetAuditTrail(int id)
//        {
//            AuditTrail auditTrail = await db.AuditTrail.FindAsync(id);
//            if (auditTrail == null)
//            {
//                return NotFound();
//            }

//            return Ok(auditTrail);
//        }

//        // PUT: api/AuditTrails/5
//        [ResponseType(typeof(void))]
//        public async Task<IHttpActionResult> PutAuditTrail(int id, AuditTrail auditTrail)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            if (id != auditTrail.Id)
//            {
//                return BadRequest();
//            }

//            db.Entry(auditTrail).State = EntityState.Modified;

//            try
//            {
//                await db.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!AuditTrailExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return StatusCode(HttpStatusCode.NoContent);
//        }

//        // POST: api/AuditTrails
//        [ResponseType(typeof(AuditTrail))]
//        public async Task<IHttpActionResult> PostAuditTrail(AuditTrail auditTrail)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            db.AuditTrail.Add(auditTrail);
//            await db.SaveChangesAsync();

//            return CreatedAtRoute("DefaultApi", new { id = auditTrail.Id }, auditTrail);
//        }

//        // DELETE: api/AuditTrails/5
//        [ResponseType(typeof(AuditTrail))]
//        public async Task<IHttpActionResult> DeleteAuditTrail(int id)
//        {
//            AuditTrail auditTrail = await db.AuditTrail.FindAsync(id);
//            if (auditTrail == null)
//            {
//                return NotFound();
//            }

//            db.AuditTrail.Remove(auditTrail);
//            await db.SaveChangesAsync();

//            return Ok(auditTrail);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private bool AuditTrailExists(int id)
//        {
//            return db.AuditTrail.Count(e => e.Id == id) > 0;
//        }
//    }
//}