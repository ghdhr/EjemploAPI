using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ICBackend.DAL;
using ICBackend.Models;

namespace ICBackend.Controllers
{
    public class StudentsController : ApiController
    {
        //Se crea un Objecto conexion de DB
        private ContosoUniversityDataEntities1 db = new ContosoUniversityDataEntities1();

        // GET: api/Students
        public List<Sp_Student_Retrieve_Result> GetStudents(int? id = null, string key = null)
        {
            return db.Sp_Student_Retrieve(id, key).ToList();
        }
        //public List<Sp_Student_Retrieve_Result> GetStudents(int? id = null, string key = null) => db.Sp_Student_Retrieve(id, key).ToList();

        // PUT: api/Students/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStudent(StudentAdd student)
        {
            //Se validan que los datos de entrada sean validos y que existan
            if (!ModelState.IsValid)        
                return BadRequest(ModelState);       
            if (student == null || student.StudentID <= 0 || student.StudentID == null)
                return BadRequest();
            try
            {
                //Se Actualiza en la DB llamando al Store Procedure
                db.Sp_Student_AddOrEdit(student.StudentID,
                                        student.FirstName,
                                        student.LastName,
                                        student.MiddleName,
                                        student.EnrollmentDate);
            }
            catch (DbUpdateConcurrencyException)
            {            
                //Si llega a haber error, se lanza un not found
                return NotFound();
            }
            //httpStatusCode tiene estandarizados sus retornos de informacion el OK pertenece al Status 200
            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Students
        [ResponseType(typeof(Student))]
        public IHttpActionResult PostStudent(Student student)
        {
            //Se validan que los datos de entrada sean validos y que existan
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (student == null)
                return BadRequest();

            try
            {
                //Se Actualiza en la DB llamando al Store Procedure
                db.Sp_Student_AddOrEdit(null,
                                        student.FirstName,
                                        student.LastName,
                                        student.MiddleName,
                                        student.EnrollmentDate);
            }
            catch (DbUpdateConcurrencyException)
            {
                //Si llega a haber error, se lanza un not found
                return NotFound();
            }
            return CreatedAtRoute("DefaultApi", new { id = student.StudentID }, student);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(Student))]
        public IHttpActionResult DeleteStudent(int id)
        {
            //Se busca el estudiante a borrar
            Student student = db.Students.Find(id);
            //Si no existe se notifica con un NotFound
            if (student == null)            
                return NotFound();

            //Se ejecuta el store Procedure
            db.Sp_Student_Delete(id);
            //Se retorna un status 200 con el student eliminado
            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.StudentID == id) > 0;
        }
    }
}