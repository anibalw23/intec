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
    public class ModuloController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult ModuloDiplomado(int diplomadoID)
        {
            ViewBag.diplomadoID = diplomadoID;
            var moduloes = db.Moduloes.Where(d => d.DiplomadoID ==  diplomadoID).Include(m => m.Diplomado);
            return View(moduloes.ToList());
        }


        // GET: Modulo
        public ActionResult Index()
        {
            var moduloes = db.Moduloes.Include(m => m.Diplomado);
            return View(moduloes.ToList());
        }

        // GET: Modulo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modulo modulo = db.Moduloes.Find(id);
            if (modulo == null)
            {
                return HttpNotFound();
            }
            return View(modulo);
        }

        // GET: Modulo/Create
        public ActionResult Create(int diplomadoID)
        {
            ViewBag.diplomadoID = diplomadoID;
            return View();
        }

        // POST: Modulo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,nombre,DiplomadoID")] Modulo modulo)
        {
           string Ntalleres = Request.Form["NumeroTalleres"];

            int orden = db.Tallers.Where(m => m.Modulo.DiplomadoID == modulo.DiplomadoID).Count();
            if (ModelState.IsValid)
            {
                db.Moduloes.Add(modulo);
                db.SaveChanges();

                if (Ntalleres != null && Ntalleres != "")
                {
                    int num = Convert.ToInt32(Ntalleres);
                    for (var k = 0; k < num; k++)
                    {
                        Taller taller = new Taller();
                        taller.ModuloID = modulo.ID;
                        taller.nombre = "Taller " + (k + 1).ToString();
                        taller.orden = (orden + k + 1);
                        db.Tallers.Add(taller);
                    }

                }
                db.SaveChanges();

                return RedirectToAction("Details", "Diplomado", new { id =  modulo.DiplomadoID });
            }

            ViewBag.DiplomadoID = new SelectList(db.Diplomadoes, "ID", "nombre", modulo.DiplomadoID);
            return View(modulo);
        }

        // GET: Modulo/Edit/5
        public ActionResult Edit(int? id, int diplomadoID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modulo modulo = db.Moduloes.Find(id);
            if (modulo == null)
            {
                return HttpNotFound();
            }
            ViewBag.diplomadoID = diplomadoID;
            return View(modulo);
        }

        // POST: Modulo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,DiplomadoID")] Modulo modulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Diplomado", new { id = modulo.DiplomadoID });
            }
            ViewBag.DiplomadoID = new SelectList(db.Diplomadoes, "ID", "nombre", modulo.DiplomadoID);
            return View(modulo);
        }

        // GET: Modulo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modulo modulo = db.Moduloes.Find(id);
            if (modulo == null)
            {
                return HttpNotFound();
            }
            return View(modulo);
        }

        // POST: Modulo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Modulo modulo = db.Moduloes.Find(id);
            db.Moduloes.Remove(modulo);
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
