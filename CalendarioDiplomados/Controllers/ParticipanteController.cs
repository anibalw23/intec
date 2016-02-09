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
using System.Text;
using System.IO;
using System.Data.OleDb;

namespace CalendarioDiplomados.Controllers
{
    [Authorize]
    public class ParticipanteController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Participante
        public ActionResult Index()
        {
            return View(db.Participantes.ToList());
        }

        public ActionResult ParticipanteAsistencia(int grupoID, int participanteID)
        {

            var calendariosGrupo = db.Calendarios.AsNoTracking().Select(x => new {x.GrupoID, x.eventos }).Where(g => g.GrupoID == grupoID).FirstOrDefault();
            List<Evento> eventos = calendariosGrupo.eventos.ToList();
            List<Ausencia> ausencias = new List<Ausencia>();
            List<AsistenciaVM> asistencias = new List<AsistenciaVM>();

            string participanteNombre = db.Participantes.Find(participanteID).nombre;


            foreach (var evt in eventos) {
                AsistenciaVM asistencia = new AsistenciaVM();
                asistencia.eventoID = evt.ID;
                asistencia.participanteID = participanteID;
                asistencia.fecha = evt.fechaIncicio.ToShortDateString();
                asistencia.asistio = db.Ausencias.AsNoTracking().Select(x => new {x.eventoID, x.participanteID }).Where(g => g.eventoID == evt.ID).Any(p => p.participanteID == participanteID) ? false : true;
                asistencias.Add(asistencia);
            }
            ViewBag.eventos = asistencias;
            return View();
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
        public ActionResult DesinscribirParticipantes(int grupoID, int[] participantes)
        {
            Grupo grupo = db.Grupoes.Find(grupoID);
            foreach(var participanteId in participantes){
                Participante participanteTemp = db.Participantes.Find(participanteId);
                grupo.participantes.Remove(participanteTemp);
                db.SaveChanges();
            }
            var JsonData = new
            {
                data = "Ok"                
            };

            return Json(JsonData.data, JsonRequestBehavior.AllowGet);
        }


        //Inscribir Participantes desde un archivo de Excel
        public ActionResult ParticipanteInscripcionExcel(int grupoID)
        {
            Grupo grupo = db.Grupoes.Find(grupoID);

            return PartialView(grupo);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult InscribirParticipanteFromExcelFile(HttpPostedFileBase uploadFile)
        {
            StringBuilder textoEmail = new StringBuilder();
            List<InscripcionVM> inscripciones = new List<InscripcionVM>();
            Grupo grupo = new Grupo();
            int grupoId = 0;
            try
            {
                grupoId = Convert.ToInt32(Request["id"]);
                grupo = db.Grupoes.Find(grupoId);
                if (uploadFile.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads/"), Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(filePath);
                    DataSet ds = new DataSet();
                    string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";

                    using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
                    {
                        conn.Open();
                        using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
                        {
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            string query = "SELECT * FROM [" + sheetName + "]";
                            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                            //DataSet ds = new DataSet();
                            adapter.Fill(ds, "Items");
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string cedula = "";
                                    string nombre = "";
                                    string telefono = "";

                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        InscripcionVM inscripcion = new InscripcionVM();
                                       
                                        nombre = ds.Tables[0].Rows[i][0].ToString();
                                        cedula = ds.Tables[0].Rows[i][1].ToString();
                                        telefono = ds.Tables[0].Rows[i][2].ToString();

                                        inscripcion.cedula = cedula;
                                        inscripcion.nombre = nombre;
                                        inscripcion.telefono = telefono;

                                        inscripciones.Add(inscripcion);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception exp)
            {
                var dummy = exp.Message;
            }

            foreach (var i in inscripciones) {
                Participante participante = new Participante();
                participante.cedula = i.cedula;
                participante.nombre = i.nombre;
                participante.telefono = i.telefono;
                bool isRepeated = grupo.participantes.Any(c => c.cedula == participante.cedula);
                if (!isRepeated) // Si no esta repetida la cedula
                {
                    grupo.participantes.Add(participante);  
                }                         
            }
            db.SaveChanges();

            return RedirectToAction("Details", "Grupo", new { id = grupoId });
        
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
