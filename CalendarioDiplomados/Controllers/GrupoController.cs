using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CalendarioDiplomados.Models;
using CalendarioDiplomados.Models.ViewModels;

namespace CalendarioDiplomados.Controllers
{
    [Authorize]
    public class GrupoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult GruposDiplomado(int diplomadoId)
        {

            ViewBag.diplomadoID = diplomadoId;
            var grupoes = db.Grupoes.Where(dp => dp.DiplomadoID == diplomadoId).Include(g => g.Diplomado);
            return PartialView(grupoes.ToList());
        }


        public ActionResult GrupoAsistencia(int grupoId)
        {
            if (grupoId == null || grupoId == 0) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var calendariosGrupo = db.Calendarios.AsNoTracking().Select(x => new { x.GrupoID, eventos = x.eventos.Select(z => new { z.ID, z.fechaIncicio, z.CalendarioID, z.orden }) }).Where(g => g.GrupoID == grupoId).FirstOrDefault();
            var fechasEventos = calendariosGrupo != null ? calendariosGrupo.eventos.Select(x => new { x.ID, fechaIncicio = x.fechaIncicio.Day + "/" + x.fechaIncicio.Month.ToString() + "/" + x.fechaIncicio.Year, x.CalendarioID, x.orden }) : null;


            if (fechasEventos != null)
            {
                ViewBag.fechas = new SelectList(fechasEventos.OrderBy(o => o.orden), "ID", "fechaIncicio");
            }
            else {
                List<Evento> eventos = new List<Evento>();
                SelectList selectList = new SelectList(eventos.Select(x => new { x.ID, x.fechaIncicio }), "ID", "fechaIncicio");
                ViewBag.fechas = selectList;
            }
           
            ViewBag.grupoId = grupoId;
            return View();
        }



        public JsonResult getAsistenciaParticipantesGrupo(int grupoId, int eventoId)
        {
            List<Ausencia> ausencias = new List<Ausencia>();
            // List<AsistenciaVM> asistencias = new List<AsistenciaVM>();
            List<AsistenciaGrupoVm> asistencias = new List<AsistenciaGrupoVm>();
            var participantes = db.Participantes.Select(x => new { x.ID, x.cedula, x.nombre, grupos = x.grupos.Select(y => new { y.ID }) }).Where(g => g.grupos.Any(x => x.ID == grupoId));


            foreach (var participante in participantes)
            {
                AsistenciaGrupoVm asistencia = new AsistenciaGrupoVm();
                asistencia.cedula = participante.cedula;
                asistencia.nombre = participante.nombre;
                asistencia.asistioManana = db.Ausencias.AsNoTracking().Select(x => new { x.eventoID, x.participanteID, x.TandaAusencia }).Where(g => g.eventoID == eventoId).Where(m => m.TandaAusencia == TandaAusencia.manana).Any(p => p.participanteID == participante.ID) ? false : true;
                asistencia.asistioTarde = db.Ausencias.AsNoTracking().Select(x => new { x.eventoID, x.participanteID, x.TandaAusencia }).Where(g => g.eventoID == eventoId).Where(m => m.TandaAusencia == TandaAusencia.tarde).Any(p => p.participanteID == participante.ID) ? false : true;
                asistencia.participanteId = participante.ID;
                asistencias.Add(asistencia);
            }

            var data = asistencias.Select(x => new { x.participanteId, x.cedula, x.nombre, x.asistioManana, x.asistioTarde }).OrderBy(n => n.nombre);
            return Json(data, JsonRequestBehavior.AllowGet);
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
        public ActionResult Create(int diplomadoID)
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
                return RedirectToAction("Details", "Diplomado", new { id = grupo.DiplomadoID });
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
