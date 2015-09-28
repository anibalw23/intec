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
    public class DiaSemanaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DiaSemana
        public ActionResult Index()
        {
            return View(db.DiaSemanas.ToList());
        }

        // GET: DiaSemana/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaSemana diaSemana = db.DiaSemanas.Find(id);
            if (diaSemana == null)
            {
                return HttpNotFound();
            }
            return View(diaSemana);
        }

        // GET: DiaSemana/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DiaSemana/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,nombre,numero")] DiaSemana diaSemana)
        {
            if (ModelState.IsValid)
            {
                db.DiaSemanas.Add(diaSemana);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(diaSemana);
        }

        // GET: DiaSemana/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaSemana diaSemana = db.DiaSemanas.Find(id);
            if (diaSemana == null)
            {
                return HttpNotFound();
            }
            return View(diaSemana);
        }

        // POST: DiaSemana/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,numero")] DiaSemana diaSemana)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diaSemana).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(diaSemana);
        }

        // GET: DiaSemana/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaSemana diaSemana = db.DiaSemanas.Find(id);
            if (diaSemana == null)
            {
                return HttpNotFound();
            }
            return View(diaSemana);
        }

        // POST: DiaSemana/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiaSemana diaSemana = db.DiaSemanas.Find(id);
            db.DiaSemanas.Remove(diaSemana);
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
