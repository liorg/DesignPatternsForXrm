using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class ValuesTestController: ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>
        public string Get(string input)
        {
            var session = HttpContext.Current.Session;
            if (session != null)
            {
                if (session["Time"] == null)
                    session["Time"] = DateTime.Now;
                return "Session Time: " + session["Time"] + input;
            }
            return "Session is not availabe" + input;
        }
        // GET api/<controller>/5
       

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}