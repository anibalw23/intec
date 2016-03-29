using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CalendarioDiplomados.Models;
using System.Threading.Tasks;

namespace CalendarioDiplomados.Controllers
{
    [Authorize]
    public class AusenciaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ausencia
        public ActionResult Index()
        {
            var ausencias = db.Ausencias.Include(a => a.Evento).Include(a => a.Participante);
            return View(ausencias.ToList());
        }

        // GET: Ausencia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ausencia ausencia = db.Ausencias.Find(id);
            if (ausencia == null)
            {
                return HttpNotFound();
            }
            return View(ausencia);
        }


        [HttpPost]
        public async Task<ActionResult> DeleteAusencia(int eventoID, int participanteID, int tandaAusencia)
        {

            if (eventoID != 0 && participanteID != 0)
            {
                Ausencia ausencia = new Ausencia();
                TandaAusencia TandaAusencia = (TandaAusencia)tandaAusencia;
                ausencia = await db.Ausencias.Where(e => e.eventoID == eventoID).Where(p => p.participanteID == participanteID).Where(t => t.TandaAusencia == TandaAusencia).FirstOrDefaultAsync();
                if (ausencia != null)
                {
                    db.Ausencias.Remove(ausencia);
                    await db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }








        [HttpPost]
        public async Task<ActionResult> CreateAusencia(int eventoID, int participanteID, int tandaAusencia)
        {

            if (eventoID != 0 && participanteID != 0)
            {
                Ausencia ausencia = new Ausencia();
                ausencia.eventoID = eventoID;
                ausencia.participanteID = participanteID;
                ausencia.TandaAusencia = (TandaAusencia)tandaAusencia;
                int isRepeated = await db.Ausencias.Select(x => new { x.eventoID, x.participanteID, x.TandaAusencia }).Where(e => e.eventoID == eventoID).Where(p => p.participanteID == participanteID).Where(t => t.TandaAusencia == ausencia.TandaAusencia).CountAsync();
                if (isRepeated < 1)
                {
                    db.Ausencias.Add(ausencia);
                    await db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }




        // GET: Ausencia/Create
        public ActionResult Create()
        {
            ViewBag.eventoID = new SelectList(db.Eventoes, "ID", "ID");
            ViewBag.participanteID = new SelectList(db.Participantes, "ID", "cedula");
            return View();
        }

        // POST: Ausencia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,eventoID,participanteID")] Ausencia ausencia)
        {
            if (ModelState.IsValid)
            {
                db.Ausencias.Add(ausencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.eventoID = new SelectList(db.Eventoes, "ID", "ID", ausencia.eventoID);
            ViewBag.participanteID = new SelectList(db.Participantes, "ID", "cedula", ausencia.participanteID);
            return View(ausencia);
        }

        // GET: Ausencia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ausencia ausencia = db.Ausencias.Find(id);
            if (ausencia == null)
            {
                return HttpNotFound();
            }
            ViewBag.eventoID = new SelectList(db.Eventoes, "ID", "ID", ausencia.eventoID);
            ViewBag.participanteID = new SelectList(db.Participantes, "ID", "cedula", ausencia.participanteID);
            return View(ausencia);
        }

        // POST: Ausencia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,eventoID,participanteID")] Ausencia ausencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ausencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.eventoID = new SelectList(db.Eventoes, "ID", "ID", ausencia.eventoID);
            ViewBag.participanteID = new SelectList(db.Participantes, "ID", "cedula", ausencia.participanteID);
            return View(ausencia);
        }

        // GET: Ausencia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ausencia ausencia = db.Ausencias.Find(id);
            if (ausencia == null)
            {
                return HttpNotFound();
            }
            return View(ausencia);
        }

        // POST: Ausencia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ausencia ausencia = db.Ausencias.Find(id);
            db.Ausencias.Remove(ausencia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
