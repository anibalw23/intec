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
    public class EventoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpPost]
        public JsonResult GetEventosDiplomado(int diplomadoID)
        {
            List<Calendario> calendarios = new List<Calendario>();
            List<Evento> eventos = new List<Evento>();
            List<Grupo> grupos = new List<Grupo>();


            calendarios = db.Calendarios.Where(d => d.Grupo.DiplomadoID == diplomadoID).Include(e => e.eventos).ToList();
            grupos = db.Grupoes.Where(d => d.DiplomadoID == diplomadoID).ToList();

            foreach (var cal in calendarios)
            {
                foreach (var evt in cal.eventos)
                {
                    if (evt.TallerID != null)
                    {
                        evt.Taller = db.Tallers.Find(evt.TallerID);
                    }
                    if(evt.FacilitadorID != null){
                     evt.Facilitador.nombre = db.Facilitadors.Find(evt.FacilitadorID).nombre;
                    }                   
                    eventos.Add(evt);
                }
            }
            
            var data = eventos;

            var fechas = eventos.OrderBy(f => f.fechaIncicio).Select(f => f.fechaIncicio.ToShortDateString()).Distinct().ToArray();
            var result = data.Select(x => new { id = x.ID.ToString(), resourceId = x.Calendario.GrupoID, resourceName = x.Calendario.Grupo.nombre, start = x.fechaIncicio.Date.ToShortDateString(), end = x.fechaFin.Date.ToString("yyyy-MM-dd"), title = "Taller " + x.Calendario.Grupo.nombre, tallerNombre = x.Taller.Modulo.nombre + " " + x.Taller.nombre }).OrderBy(r => r.resourceId);
            var resources = grupos.Select(x => new { id = x.ID, title = x.nombre, participantes = x.cantidadParticipantes});
            
            var jsonData = new
            {
                fechas = fechas,
                result,                
                resources
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Informe() {
            return View();
        }


        public JsonResult getEventosByDiplomado()
        {
            List<Evento> pcs = new List<Evento>();
            var eventos = db.Eventoes.OrderBy(f => f.fechaIncicio).Select(x => new { fecha = x.fechaIncicio.Year + "(Año)/" + x.fechaIncicio.Month + "(Mes)/" + x.fechaIncicio.Day + " (Dia)", taller = x.Taller.Modulo.nombre + " " + x.Taller.nombre, grupo = x.Calendario.Grupo.nombre }).ToList();
            
            var jsonData = new
            {
                Records = pcs
            };

            return Json(new { Result = "OK", Records = eventos }, JsonRequestBehavior.AllowGet);

        }
















        public ActionResult EventoCalendario(int calendarioID)
        {
            var eventoes = db.Eventoes.Where(c => c.CalendarioID == calendarioID).Include(f => f.Facilitador).Include(t => t.Taller).Include(e => e.Calendario).OrderBy(f => f.fechaIncicio);
            Calendario calendario = db.Calendarios.Find(calendarioID);

            List<Taller> talleres = db.Tallers.Where(d => d.Modulo.DiplomadoID == calendario.Grupo.DiplomadoID).ToList();
            foreach (var e in eventoes)
            {
                if (e.TallerID != null)
                {
                    Taller taller = db.Tallers.Find(e.TallerID);
                    talleres.Remove(taller);
                }
            }

            ViewBag.TalleresEventos = talleres;
            ViewBag.Talleres = db.Tallers.Where(d => d.Modulo.DiplomadoID == calendario.Grupo.DiplomadoID).ToList();
            return PartialView(eventoes.ToList());
        }




        public ActionResult EditarTaller(int id, int calendarioID)
        {
            List<Facilitador> facilitadores = new List<Facilitador>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventoes.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }

            Calendario calendario = db.Calendarios.Find(calendarioID);
            ViewBag.CalendarioID = calendarioID;

            var talleres = db.Tallers.Where(d => d.Modulo.DiplomadoID == calendario.Grupo.DiplomadoID).ToList();

            //Editar Taller Seleccionado
            var items = new List<SelectListItem>();
            foreach (var taller in talleres)
            {
                items.Add(new SelectListItem() { Selected = evento.TallerID.Equals(taller.ID), Text = taller.Modulo.nombre + "-" + taller.nombre, Value = taller.ID.ToString() });

                if (taller.FacilitadorID != null && taller.FacilitadorID != 0)
                {
                    Facilitador facilitador = db.Facilitadors.Find(taller.FacilitadorID);
                    facilitadores.Add(facilitador);
                }
            }
            facilitadores = facilitadores.Distinct().ToList();

            //Editar Facilitador Seleccionado
            var itemsFacilitador = new List<SelectListItem>();
            foreach(var facilitador in facilitadores){
                itemsFacilitador.Add(new SelectListItem() { Selected = evento.FacilitadorID.Equals(facilitador.ID), Text = facilitador.nombre , Value = facilitador.ID.ToString() });
            }




            ViewBag.TallerID = new SelectList(items, "Value", "Text", evento.TallerID);
            ViewBag.FacilitadorID = new SelectList(itemsFacilitador, "Value", "Text", evento.FacilitadorID);
            //ViewBag.FacilitadorID = new SelectList(facilitadores, "ID", "nombre");
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarTaller(Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.fechaFin = evento.fechaIncicio;
                db.Entry(evento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Calendario", new { id = evento.CalendarioID });
            }
            ViewBag.CalendarioID = new SelectList(db.Calendarios, "ID", "nombre", evento.CalendarioID);
            ViewBag.TallerID = new SelectList(db.Tallers, "ID", "nombre", evento.TallerID);
            return View(evento);

        }




        // GET: Evento
        public ActionResult Index()
        {
            var eventoes = db.Eventoes.Include(e => e.Calendario).Include(e => e.Taller);
            return View(eventoes.ToList());
        }

        // GET: Evento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventoes.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // GET: Evento/Create
        public ActionResult Create(int calendarioID)
        {
            ViewBag.CalendarioID = calendarioID;
            ViewBag.TallerID = new SelectList(db.Tallers, "ID", "nombre");
            return View();
        }

        // POST: Evento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento evento)
        {
            List<Evento> eventosCal = new List<Evento>();
            if (ModelState.IsValid)
            {

                DateTime eventoFechaAnterior = evento.fechaIncicio;
                evento.fechaFin = evento.fechaIncicio;
                db.Eventoes.Add(evento);
                db.SaveChanges();

                db.Entry(evento).State = EntityState.Detached;


                String value = Request.Form["opcionCheckbox"];
                int isCheked = Convert.ToInt32(value);

                List<int> diasEvt = getDiasEvento(evento);
                if (isCheked == 1)
                {
                    //Si cambia de fecha
                    Calendario calendario = db.Calendarios.AsNoTracking().Where(i => i.ID == evento.CalendarioID).SingleOrDefault();
                    eventosCal.AddRange(calendario.eventos.Where(ev => ev.ID != evento.ID).Where(f => f.fechaIncicio >= eventoFechaAnterior).OrderBy(f => f.fechaIncicio).ToList());

                    //Prueba Temporal de  Algoritmo de desplazamiento
                    List<DateTime> eventosDeplazados = desplazarEventos(eventosCal, diasEvt, evento.fechaIncicio, 1);

                    int evtIndex = 0;
                    foreach (var evt in eventosCal)
                    {
                        evt.fechaIncicio = Convert.ToDateTime(eventosDeplazados[evtIndex]);
                        evt.fechaFin = evt.fechaIncicio;
                        db.Entry(evt).State = EntityState.Modified;
                        foreach (var d in diasEvt)
                        {
                            switch (d)
                            {
                                case 0:
                                    evt.isDomingo = true;
                                    break;
                                case 1:
                                    evt.isLunes = true;
                                    break;
                                case 2:
                                    evt.isMartes = true;
                                    break;
                                case 3:
                                    evt.isMiercoles = true;
                                    break;
                                case 4:
                                    evt.isJueves = true;
                                    break;
                                case 5:
                                    evt.isViernes = true;
                                    break;
                                case 6:
                                    evt.isSabado = true;
                                    break;
                            }
                        }
                        evtIndex++;
                    }
                    db.SaveChanges();

                    // --- Actualiza datos del Calendario ---
                    DateTime dateMin = new DateTime();
                    DateTime dateMax = new DateTime();

                    dateMin = db.Eventoes.AsNoTracking().Where(c => c.CalendarioID == evento.CalendarioID).Min(d => d.fechaIncicio);
                    dateMax = db.Eventoes.AsNoTracking().Where(c => c.CalendarioID == evento.CalendarioID).Max(d => d.fechaIncicio);
                    calendario.fechaInicio = dateMin;
                    calendario.fechaFin = dateMax;
                    try { 
                        db.Entry(calendario).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch(Exception e){
                        var msj = e.Message;
                    }
                    // ----- End Actualizar Datos del Calendario ---
                }



                return RedirectToAction("Details", "Calendario", new { id = evento.CalendarioID });
            }

            ViewBag.CalendarioID = new SelectList(db.Calendarios, "ID", "nombre", evento.CalendarioID);
            ViewBag.TallerID = new SelectList(db.Tallers, "ID", "nombre", evento.TallerID);
            return View(evento);
        }

        // GET: Evento/Edit/5
        public ActionResult Edit(int? id, int calendarioID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventoes.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            ViewBag.CalendarioID = calendarioID;
            ViewBag.TallerID = new SelectList(db.Tallers, "ID", "nombre", evento.TallerID);
            return View(evento);
        }

        // POST: Evento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Evento evento)
        {
            String value = Request.Form["opcionCheckbox"];
            string dia = Request.Form["diaCheckbox"];
            int isCheked = Convert.ToInt32(value);
            List<Evento> eventosCal = new List<Evento>();

            DateTime eventoFechaAnterior = db.Eventoes.AsNoTracking().Where(evt => evt.ID == evento.ID).SingleOrDefault().fechaIncicio;

            if (ModelState.IsValid)
            {
                List<int> diasEvt = getDiasEvento(evento);
                if (isCheked == 1)
                {
                    //Si cambia de fecha
                    Calendario calendario = db.Calendarios.AsNoTracking().Where(i => i.ID == evento.CalendarioID).SingleOrDefault();
                    eventosCal.AddRange(calendario.eventos.Where(ev => ev.ID == evento.ID).ToList());
                    eventosCal.AddRange(calendario.eventos.Where(ev => ev.ID != evento.ID).Where(f => f.fechaIncicio >= eventoFechaAnterior).OrderBy(f => f.fechaIncicio).ToList());

                    //Prueba Temporal de  Algoritmo de desplazamiento
                    List<DateTime> eventosDeplazados = desplazarEventos(eventosCal, diasEvt, evento.fechaIncicio, 0);

                    int evtIndex = 0;
                    foreach (var evt in eventosCal)
                    {
                        evt.fechaIncicio = Convert.ToDateTime(eventosDeplazados[evtIndex]);
                        evt.fechaFin = evt.fechaIncicio;

                        foreach (var d in diasEvt)
                        {
                             switch(d){
                                 case 0:
                                     evt.isDomingo = true;
                                     break;
                                 case 1:
                                     evt.isLunes = true;
                                     break;
                                 case 2:
                                     evt.isMartes = true;
                                     break;
                                 case 3:
                                     evt.isMiercoles = true;
                                     break;
                                 case 4:
                                     evt.isJueves = true;
                                     break;
                                 case 5:
                                     evt.isViernes = true;
                                     break;
                                 case 6:
                                     evt.isSabado = true;
                                     break;
                             }
                        }

                        db.Entry(evt).State = EntityState.Modified;
                        evtIndex++;
                    }
                    db.SaveChanges();

                    // --- Actualiza datos del Calendario ---
                    DateTime dateMin = new DateTime();
                    DateTime dateMax = new DateTime();

                    dateMin = db.Eventoes.Where(c => c.CalendarioID == evento.CalendarioID).Min(d => d.fechaIncicio);
                    dateMax = db.Eventoes.Where(c => c.CalendarioID == evento.CalendarioID).Max(d => d.fechaIncicio);
                    calendario.fechaInicio = dateMin;
                    calendario.fechaFin = dateMax;
                    db.Entry(calendario).State = EntityState.Modified;
                    db.SaveChanges();
                    // ----- End Actualizar Datos del Calendario ---

                }
                else {
                    evento.fechaFin = evento.fechaIncicio;
                    db.Entry(evento).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Details", "Calendario", new { id = evento.CalendarioID });
            }
            ViewBag.CalendarioID = new SelectList(db.Calendarios, "ID", "nombre", evento.CalendarioID);
            ViewBag.TallerID = new SelectList(db.Tallers, "ID", "nombre", evento.TallerID);
            return View(evento);
        }

        public List<DateTime> desplazarEventos(List<Evento> eventos, List<int> dias, DateTime fechaInicio, int startWeek)
        {
            List<Evento> eventosResult = new List<Evento>();
            List<DateTime> nextFechas = new List<DateTime>();
            int weekNumber = startWeek;
            int moduloCounter = 1;
            dias.Sort();

                foreach (var evt in eventos)
                {

                    bool isRepeated = eventosResult.Any(c => c.ID == evt.ID);
                    if (!isRepeated && dias.Count() > 0)
                    {
                        //int diaDiferencia = Math.Abs(((int)fechaInicio.DayOfWeek) - (dias[moduloCounter - 1]));
                        int diaDiferencia = ((int)fechaInicio.DayOfWeek) - (dias[moduloCounter - 1]);
                        if (diaDiferencia <= 0)
                        {
                             diaDiferencia =  Math.Abs(diaDiferencia);
                        }
                        else {
                            diaDiferencia = 7 - Math.Abs(diaDiferencia);
                        }

                        evt.fechaIncicio = fechaInicio.AddDays(diaDiferencia).AddDays(weekNumber * 7);
                        evt.fechaFin = evt.fechaIncicio;
                        eventosResult.Add(evt);
                        //Incrementa el numero de semanas
                        if (moduloCounter % dias.Count() == 0)
                        {
                            moduloCounter = 0;
                            weekNumber++;
                        }
                        moduloCounter++;
                    }
                }

                nextFechas = eventosResult.OrderBy(f => f.fechaIncicio).Select(x => x.fechaIncicio).ToList();

                return nextFechas;
        }


        public List<int> getDiasEvento(Evento evento)
        {
            List<int> dias = new List<int>();
            if (evento.isLunes)
            {
                dias.Add(1);
            }
            if (evento.isMartes)
            {
                dias.Add(2);
            }
            if (evento.isMiercoles)
            {
                dias.Add(3);
            }
            if (evento.isJueves)
            {
                dias.Add(4);
            }
            if (evento.isViernes)
            {
                dias.Add(5);
            }
            if (evento.isSabado)
            {
                dias.Add(6);
            }
            if (evento.isDomingo)
            {
                dias.Add(0);
            }
            return dias;
        }


        // GET: Evento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventoes.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // POST: Evento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evento evento = db.Eventoes.Find(id);
            db.Eventoes.Remove(evento);
            db.SaveChanges();
            return RedirectToAction("Details", "Calendario", new { id = evento.CalendarioID });
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
