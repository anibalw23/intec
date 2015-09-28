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
    public class FacilitadorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult FacilitadorDiplomado(int diplomadoID)
        {
            ViewBag.DiplomadoID = diplomadoID;
            List<Facilitador> facilitadores = db.Facilitadors.Where(d => d.talleres.Any(p => p.Modulo.DiplomadoID == diplomadoID)).Include(t => t.talleres).ToList();
            return PartialView(facilitadores);
        }


        // GET: Facilitador
        public ActionResult Index()
        {
            return View(db.Facilitadors.ToList());
        }

        // GET: Facilitador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facilitador facilitador = db.Facilitadors.Find(id);
            if (facilitador == null)
            {
                return HttpNotFound();
            }
            return View(facilitador);
        }

        // GET: Facilitador/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Facilitador/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,cedula,nombre")] Facilitador facilitador)
        {
            if (ModelState.IsValid)
            {
                db.Facilitadors.Add(facilitador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facilitador);
        }

        // GET: Facilitador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facilitador facilitador = db.Facilitadors.Find(id);
            if (facilitador == null)
            {
                return HttpNotFound();
            }
            return View(facilitador);
        }

        // POST: Facilitador/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,cedula,nombre")] Facilitador facilitador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facilitador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(facilitador);
        }

        // GET: Facilitador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facilitador facilitador = db.Facilitadors.Find(id);
            if (facilitador == null)
            {
                return HttpNotFound();
            }
            return View(facilitador);
        }

        // POST: Facilitador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Facilitador facilitador = db.Facilitadors.Find(id);
            db.Facilitadors.Remove(facilitador);
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
