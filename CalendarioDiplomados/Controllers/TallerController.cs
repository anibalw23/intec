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
    public class TallerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult TallerFacilitador(int id)
        {
            Taller taller = db.Tallers.Find(id);
            ViewBag.FacilitadorID = new SelectList(db.Facilitadors, "ID", "nombre");
            return View(taller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TallerFacilitador(Taller taller)
        {

            if (ModelState.IsValid)
            {
                db.Entry(taller).State = EntityState.Modified;
                db.SaveChanges();

                Modulo modulo = db.Moduloes.Find(taller.ModuloID);

                return RedirectToAction("Details", "Diplomado", new {id = modulo.DiplomadoID });
            }
            ViewBag.FacilitadorID = new SelectList(db.Facilitadors, "ID", "nombre");
            return View(taller);
        }



        public ActionResult TallerDiplomado(int diplomadoID)
        {
            ViewBag.diplomadoID = diplomadoID;
            var tallers = db.Tallers.Where(dp => dp.Modulo.DiplomadoID ==  diplomadoID).Include(t => t.Modulo);
            return PartialView(tallers.ToList());
        }

        // GET: Taller
        public ActionResult Index()
        {
            var tallers = db.Tallers.Include(t => t.Modulo);
            return View(tallers.ToList());
        }

        // GET: Taller/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taller taller = db.Tallers.Find(id);
            if (taller == null)
            {
                return HttpNotFound();
            }
            return View(taller);
        }

        // GET: Taller/Create
        public ActionResult Create( int diplomadoID)
        {
            ViewBag.ModuloID = new SelectList(db.Moduloes.Where(dp => dp.DiplomadoID == diplomadoID), "ID", "nombre");
            return View();
        }

        // POST: Taller/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,nombre,ModuloID,orden,FacilitadorID,cantidadDias")] Taller taller)
        {
            if (ModelState.IsValid)
            {
                Modulo modulo = db.Moduloes.Find(taller.ModuloID);
                int orden = db.Tallers.Where(m => m.Modulo.DiplomadoID == modulo.DiplomadoID).Count();
                taller.orden = (orden + 1);
                db.Tallers.Add(taller);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ModuloID = new SelectList(db.Moduloes.Where(dp => dp.DiplomadoID == taller.Modulo.DiplomadoID), "ID", "nombre");
            return View(taller);
        }

        // GET: Taller/Edit/5
        public ActionResult Edit(int? id, int diplomadoID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taller taller = db.Tallers.Find(id);
            if (taller == null)
            {
                return HttpNotFound();
            }

            List<Modulo> modulos = new List<Modulo>();
            modulos = db.Moduloes.Where(dp => dp.DiplomadoID == diplomadoID).ToList();
            
            List<SelectListItem> list = new List<SelectListItem> ();
            foreach(var mod in modulos){            
               list.Add(new SelectListItem() { Text = mod.nombre, Value = mod.ID.ToString(), Selected = (mod.ID == taller.ModuloID) });
            }

            ViewBag.ModuloID = list;
            ViewBag.diplomadoID = diplomadoID;
            return View(taller);
        }

        // POST: Taller/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,ModuloID,orden,FacilitadorID,cantidadDias,diplomadoId")] Taller taller)
        {
            string diplomado = Request.Form["diplomadoId"];
            int diplomadoId = Convert.ToInt32(diplomado);
            if (ModelState.IsValid)
            {
                db.Entry(taller).State = EntityState.Modified;
                db.SaveChanges();

                //var diplomadoId = db.Moduloes.Where(t => t.talleres.Any(i => i.ID == taller.ID)).SingleOrDefault().DiplomadoID;

                return RedirectToAction("Details", "Diplomado", new { id = diplomadoId });
            }
            ViewBag.ModuloID = new SelectList(db.Moduloes.Where(dp => dp.DiplomadoID == taller.Modulo.DiplomadoID), "ID", "nombre");
            return View(taller);
        }

        // GET: Taller/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taller taller = db.Tallers.Find(id);
            if (taller == null)
            {
                return HttpNotFound();
            }
            return View(taller);
        }

        // POST: Taller/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Taller taller = db.Tallers.Find(id);
            db.Tallers.Remove(taller);
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
