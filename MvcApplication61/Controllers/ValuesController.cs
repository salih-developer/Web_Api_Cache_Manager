using System;
using System.Web.Http;

namespace MvcApplication61.Controllers
{
    using System.Web.Http.Results;
    using System.Web.Mvc;

    using DeFacto.V2.Api.Customer;

    public class Personel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateTime { get; set; }
    }



    public class ValuesController : ApiController
    {

        [OutputCacheManager]
        public JsonResult<Personel> Get(int id)
        {
            var data = new Personel { Id = id, DateTime = DateTime.Now, Name = "test" };
            return this.Json<Personel>(data);
        }
    }
}