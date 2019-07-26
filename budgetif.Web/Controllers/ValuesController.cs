using budgetif.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace budgetif.Web.Controllers
{
    public class ValuesController : ApiController
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // GET api/values
        public IEnumerable<string> Get()
        {
            var db = new BudgetIfContext();
            var total = db.Transactions.Sum(p => p.amount);
            var text = $"За серпень 2017 року витратили {total:#.00} гривень... \n\r/kikokiko";

            logger.Debug("GET VALUES");
            return new string[] { text, "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
