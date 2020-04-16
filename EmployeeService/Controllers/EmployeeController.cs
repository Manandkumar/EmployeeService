using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess;

namespace EmployeeService.Controllers
{
    public class EmployeeController : ApiController
    {
       /* public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

         public HttpResponseMessage Get(int id)
          {
              using (EmployeeDBEntities entities = new EmployeeDBEntities())
              {
                  var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                  if (entity != null)
                  {
                      return Request.CreateResponse(HttpStatusCode.OK, entity);
                  }
                  else
                  {
                      return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Employee with ID = " + id.ToString() + " not found");
                  }
              }
          }*/
        public HttpResponseMessage Get(string gender ="all")
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value for Gender must be All / Male / Female. " + gender + " is Invalid. ");




                }
               
            }
        }
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            using(EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                try
                {

                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee ID =" + id.ToString() + " is not found");

                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                catch (Exception Ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Ex);
                }
            }
        }
        public HttpResponseMessage Put([FromBody] int id, [FromUri] Employee employee)
        //public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())

            {

                try
                {

                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee ID with " + id.ToString() + " Not Found!!");

                    }

                    else
                    {

                        entity.FirstName = employee.FirstName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;
                        entity.LastName = employee.LastName;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);

                    }
                }
                catch (Exception Ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Ex);
                }

                
                
            }
        }

    }

}
