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
    public class ChoferController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chofer
        public ActionResult Index()
        {
            return View(db.Chofers.ToList());
        }

        // GET: Chofer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chofer chofer = db.Chofers.Find(id);
            if (chofer == null)
            {
                return HttpNotFound();
            }
            return View(chofer);
        }

        // GET: Chofer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chofer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,cedula,nombre")] Chofer chofer)
        {
            if (ModelState.IsValid)
            {
                db.Chofers.Add(chofer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chofer);
        }

        // GET: Chofer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chofer chofer = db.Chofers.Find(id);
            if (chofer == null)
            {
                return HttpNotFound();
            }
            return View(chofer);
        }

        // POST: Chofer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,cedula,nombre")] Chofer chofer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chofer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chofer);
        }

        // GET: Chofer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chofer chofer = db.Chofers.Find(id);
            if (chofer == null)
            {
                return HttpNotFound();
            }
            return View(chofer);
        }

        // POST: Chofer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chofer chofer = db.Chofers.Find(id);
            db.Chofers.Remove(chofer);
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
