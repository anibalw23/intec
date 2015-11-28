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
    public class ParticipanteController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Participante
        public ActionResult Index()
        {
            return View(db.Participantes.ToList());
        }

        public ActionResult ParticipanteGrupo(int grupoID)
        {
            Grupo grupo = db.Grupoes.Find(grupoID);
            List<Participante> participantes = grupo.participantes.ToList();
            return View(participantes);
        }

        public ActionResult InscribirParticipante(int grupoID)
        {
            ViewBag.grupoID = grupoID;
            return PartialView();
        }


        [HttpPost]
        public ActionResult DesInscribirParticipante(int grupoID, int participanteID)
        {
            Grupo grupo = db.Grupoes.Find(grupoID);
            Participante participante = db.Participantes.Find(participanteID);
            try
            {
                grupo.participantes.Remove(participante);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error 505", "Ocurrio un Error al inscribir, favor contacte a su administrador!");
            }

            return RedirectToAction("Details", "Grupo", new { id = grupoID, diplomadoID = grupo.DiplomadoID });
        }





        [HttpPost]
        public ActionResult InscribirParticipante(int grupoID, string participantesID)
        {
            Grupo grupo = db.Grupoes.Find(grupoID);
            string[] participantesStrId = participantesID.Split(',');
            try
            {
                foreach (var partiId in participantesStrId)
                {
                    int participanteId = Convert.ToInt32(partiId);
                    Participante participante = db.Participantes.Find(participanteId);
                    grupo.participantes.Add(participante);
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error 505", "Ocurrio un Error al inscribir, favor contacte a su administrador!");
            }

            return RedirectToAction("Details", "Grupo", new { id = grupoID, diplomadoID = grupo.DiplomadoID });
        }



        [HttpPost]
        public JsonResult GetParticipantes()
        {
            var result = "OK";
            List<Participante> participantes = new List<Participante>();
            try
            {
                participantes = db.Participantes.ToList();
            }
            catch (Exception e)
            {
                result = "ERROR";
            }
            var JsonData = new
            {
                data = participantes.Select(c => new
                {
                    label = c.cedula,
                    title = c.nombre,
                    value = c.ID
                })
            };

            return Json(JsonData.data, JsonRequestBehavior.AllowGet);
        }


        // GET: Participante/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participante participante = db.Participantes.Find(id);
            if (participante == null)
            {
                return HttpNotFound();
            }
            return View(participante);
        }

        // GET: Participante/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateByGrupo(int grupoID)
        {
            ViewBag.grupoID = grupoID;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateByGrupo([Bind(Include = "ID,cedula,nombre,telefono,direccion,grupoID")] Participante participante)
        {
            List<Grupo> grupos = new List<Grupo>();
            string grupoIdStr = Request.Form["grupoID"];
            int grupoId = 0;
            if (grupoIdStr != null && grupoIdStr !="" )
            {
                grupoId = Convert.ToInt32(grupoIdStr);
                Grupo grupo = db.Grupoes.Find(grupoId);
                if (ModelState.IsValid)
                {
                    participante.grupos = grupos;
                    participante.grupos.Add(grupo);
                    db.Participantes.Add(participante);
                    db.SaveChanges();
                }
                else {
                    ModelState.AddModelError("Error Db","Error favor contacte a su admin");
                }

            }
           
            return RedirectToAction("Details", "Grupo", new { id = grupoId });
        }





        // POST: Participante/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,cedula,nombre,telefono,direccion")] Participante participante)
        {
            if (ModelState.IsValid)
            {
                db.Participantes.Add(participante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(participante);
        }

        // GET: Participante/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participante participante = db.Participantes.Find(id);
            if (participante == null)
            {
                return HttpNotFound();
            }
            return View(participante);
        }

        // POST: Participante/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,cedula,nombre,telefono,direccion")] Participante participante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(participante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(participante);
        }

        // GET: Participante/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participante participante = db.Participantes.Find(id);
            if (participante == null)
            {
                return HttpNotFound();
            }
            return View(participante);
        }

        // POST: Participante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Participante participante = db.Participantes.Find(id);
            db.Participantes.Remove(participante);
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
