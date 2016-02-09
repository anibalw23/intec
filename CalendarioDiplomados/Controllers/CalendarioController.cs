﻿using System;
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
    public class CalendarioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult CalendarioGrupo(int grupoID)
        {
            ViewBag.grupoID = grupoID;
            var calendarios = db.Calendarios.AsNoTracking().Where(g => g.GrupoID == grupoID).Include(c => c.Grupo);
            return PartialView(calendarios.ToList());
        }



        // GET: Calendario
        public ActionResult Index()
        {
            var calendarios = db.Calendarios.AsNoTracking().Include(c => c.Grupo);
            return View(calendarios.ToList());
        }

        // GET: Calendario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendario calendario = db.Calendarios.Find(id);
            if (calendario == null)
            {
                return HttpNotFound();
            }
            return View(calendario);
        }

        // GET: Calendario/Create
        public ActionResult Create(int grupoID)
        {
            ViewBag.grupoID = grupoID;
            return View();
        }

        // POST: Calendario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Calendario calendario)
        {


            List<Evento> eventos = new List<Evento>();
            List<Taller> talleres = new List<Taller>();
            Grupo grupo = db.Grupoes.Find(calendario.GrupoID);
            Diplomado diplomado = db.Diplomadoes.Find(grupo.DiplomadoID);


            List<Modulo> modulos = db.Moduloes.Where(c => c.DiplomadoID == grupo.DiplomadoID).ToList();
            foreach (var m in modulos)
            {
                talleres.AddRange(m.talleres);
            }
            string dia = Request.Form["diaCheckbox"];

            int eventOrder = 0;
            // int weeks = Math.Abs((Int32) (calendario.fechaInicio - calendario.fechaFin).TotalDays / 7); // so es con fecha-inicio -- fecha fin

            int diacount = 1;
            if (dia != null && dia != "")
            {
                diacount = dia.Split(',').Count();
            }
            int weeks = (talleres.Count() / (diacount)) - 1;
            float weeksDays = ((float)(talleres.Count()) / (float)diacount) - 1;
            int daysLeft = (int)((decimal)((weeksDays) % 1) * 10);
            if (daysLeft > 0)
            {
                weeks++;
            }
            if (ModelState.IsValid)
            {
                calendario.fechaFin = calendario.fechaInicio.AddDays(weeks * 7);
                db.Calendarios.Add(calendario);
                db.SaveChanges();
                int k = 0;
                for (k = 0; k <= weeks; k++)
                {
                    if (dia != null && dia != "")
                    {
                        String[] dias = dia.Split(',');
                        foreach (var d in dias)
                        {
                            Evento evento = new Evento();
                            int diaDiferencia = Math.Abs(((int)calendario.fechaInicio.DayOfWeek) - Convert.ToInt32(d));
                            int diaNumber = Convert.ToInt32(d);

                            evento.fechaIncicio = calendario.fechaInicio.AddDays(k * 7 + diaDiferencia);
                            evento.fechaFin = calendario.fechaInicio.AddDays(k * 7 + diaDiferencia);
                            evento.CalendarioID = calendario.ID;
                            evento.orden = eventOrder;
                            eventOrder++;
                            //Agrega automaticamente los talleres a los eventos
                            talleres = talleres.OrderBy(o => o.orden).ToList();
                            try
                            {
                                Taller taller = talleres.Where(o => o.orden == (evento.orden + 1)).SingleOrDefault();
                                int tallerID = taller.ID;
                                if (tallerID != 0)
                                {
                                    evento.TallerID = tallerID;
                                    if (taller.FacilitadorID != 0)
                                    {
                                        evento.FacilitadorID = taller.FacilitadorID;
                                    }
                                    foreach (var diasEvento in dias)
                                    {
                                        int diaEvt = Convert.ToInt32(diasEvento);
                                        switch (diaEvt)
                                        {
                                            case 0:
                                                evento.isDomingo = true;
                                                break;
                                            case 1:
                                                evento.isLunes = true;
                                                break;
                                            case 2:
                                                evento.isMartes = true;
                                                break;
                                            case 3:
                                                evento.isMiercoles = true;
                                                break;
                                            case 4:
                                                evento.isJueves = true;
                                                break;
                                            case 5:
                                                evento.isViernes = true;
                                                break;
                                            case 6:
                                                evento.isSabado = true;
                                                break;
                                        }
                                    }
                                    eventos.Add(evento);
                                }
                            }
                            catch (Exception e)
                            {
                                ModelState.AddModelError("", e.Message);
                            }

                        }
                    }

                }
                db.Eventoes.AddRange(eventos);
                db.SaveChanges();

                //Para actualizar la fecha de fin del diplomado
                db.Diplomadoes.Attach(diplomado);
                var entry = db.Entry(diplomado);
                bool fechaDipomadoChanged = false;
                if (diplomado.fechaFin < calendario.fechaFin)
                {
                    diplomado.fechaFin = calendario.fechaFin;
                    entry.Property(e => e.fechaFin).IsModified = true;
                    fechaDipomadoChanged = true;
                }
                if (diplomado.fechaInicio > calendario.fechaInicio)
                {
                    diplomado.fechaInicio = calendario.fechaInicio;
                    entry.Property(e => e.fechaInicio).IsModified = true;
                    fechaDipomadoChanged = true;
                }
                if (fechaDipomadoChanged)
                {
                    db.SaveChanges();
                }
                //End Actualiza fechas del diplomado
                return RedirectToAction("Details", "Grupo", new { id = calendario.GrupoID });
            }

            ViewBag.GrupoID = new SelectList(db.Grupoes, "ID", "nombre", calendario.GrupoID);
            return View(calendario);
        }




        // GET: Calendario/Edit/5
        public ActionResult Edit(int? id, int grupoID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendario calendario = db.Calendarios.Find(id);
            if (calendario == null)
            {
                return HttpNotFound();
            }
            ViewBag.grupoID = grupoID;
            return View(calendario);
        }

        // POST: Calendario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,GrupoID,fechaInicio,fechaFin")] Calendario calendario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendario).State = EntityState.Modified;
                db.SaveChanges();
                //Aqui debe modificar las fechas de los talleres igualmente
                List<Evento> eventosCal = db.Eventoes.Where(c => c.CalendarioID == calendario.ID).OrderBy(f => f.fechaIncicio).ToList();
                int k = 0;
                foreach (var evt in eventosCal)
                {
                    evt.fechaIncicio = calendario.fechaInicio.AddDays(k * 7);
                    db.Entry(evt).State = EntityState.Modified;
                    db.SaveChanges();
                    k++;
                }
                return RedirectToAction("Details", "Grupo", new { id = calendario.GrupoID });
            }
            ViewBag.GrupoID = new SelectList(db.Grupoes, "ID", "nombre", calendario.GrupoID);
            return View(calendario);
        }

        // GET: Calendario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendario calendario = db.Calendarios.Find(id);
            if (calendario == null)
            {
                return HttpNotFound();
            }
            return View(calendario);
        }

        // POST: Calendario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Calendario calendario = db.Calendarios.Find(id);
            db.Calendarios.Remove(calendario);
            db.SaveChanges();

            Grupo grupo = db.Grupoes.Find(calendario.GrupoID);
            return RedirectToAction("Details", "Grupo", new { id = grupo.ID });
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