using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CalendarioDiplomados.Models;

namespace CalendarioDiplomados.Controllers
{
    [Authorize]
    public class GrupoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult GruposDiplomado(int diplomadoId) {

            ViewBag.diplomadoID = diplomadoId;
            var grupoes = db.Grupoes.Where(dp => dp.DiplomadoID == diplomadoId).Include(g => g.Diplomado);
            return PartialView(grupoes.ToList());
        }



        // GET: Grupo
        public ActionResult Index()
        {
            var grupoes = db.Grupoes.Include(g => g.Diplomado);
            return View(grupoes.ToList());
        }

        // GET: Grupo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo = db.Grupoes.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // GET: Grupo/Create
        public ActionResult Create( int diplomadoID)
        {
            ViewBag.diplomadoID = diplomadoID;//new SelectList(db.Diplomadoes, "ID", "nombre");
            return View();
        }

        // POST: Grupo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,nombre,DiplomadoID,cantidadParticipantes")] Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                db.Grupoes.Add(grupo);
                db.SaveChanges();
                return RedirectToAction("Details", "Diplomado", new { id = grupo.DiplomadoID});
            }

            ViewBag.DiplomadoID = new SelectList(db.Diplomadoes, "ID", "nombre", grupo.DiplomadoID);
            return View(grupo);
        }

        // GET: Grupo/Edit/5
        public ActionResult Edit(int? id, int diplomadoID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo = db.Grupoes.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            ViewBag.diplomadoID = diplomadoID;//new SelectList(db.Diplomadoes, "ID", "nombre", grupo.DiplomadoID);
            return View(grupo);
        }

        // POST: Grupo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,DiplomadoID,cantidadParticipantes")] Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Diplomado", new { id = grupo.DiplomadoID });
            }
            ViewBag.DiplomadoID = new SelectList(db.Diplomadoes, "ID", "nombre", grupo.DiplomadoID);
            return View(grupo);
        }

        // GET: Grupo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo = db.Grupoes.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // POST: Grupo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Grupo grupo = db.Grupoes.Find(id);
            db.Grupoes.Remove(grupo);
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
