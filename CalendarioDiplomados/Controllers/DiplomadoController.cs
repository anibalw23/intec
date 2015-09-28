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
    public class DiplomadoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Diplomado
        public ActionResult Index()
        {
            return View(db.Diplomadoes.ToList());
        }

        // GET: Diplomado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diplomado diplomado = db.Diplomadoes.Find(id);
            if (diplomado == null)
            {
                return HttpNotFound();
            }
            return View(diplomado);
        }

        // GET: Diplomado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Diplomado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,nombre,fechaInicio,fechaFin")] Diplomado diplomado)
        {
            if (ModelState.IsValid)
            {
                db.Diplomadoes.Add(diplomado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(diplomado);
        }

        // GET: Diplomado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diplomado diplomado = db.Diplomadoes.Find(id);
            if (diplomado == null)
            {
                return HttpNotFound();
            }
            return View(diplomado);
        }

        // POST: Diplomado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,fechaInicio,fechaFin")] Diplomado diplomado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diplomado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(diplomado);
        }

        // GET: Diplomado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diplomado diplomado = db.Diplomadoes.Find(id);
            if (diplomado == null)
            {
                return HttpNotFound();
            }
            return View(diplomado);
        }

        // POST: Diplomado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Diplomado diplomado = db.Diplomadoes.Find(id);
            db.Diplomadoes.Remove(diplomado);
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
