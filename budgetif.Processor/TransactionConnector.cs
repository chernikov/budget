using budgetif.Processor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Processor
{
    public class TransactionConnector : WebConnector
    {
        private static string baseUrl = "http://api.e-data.gov.ua:8080/api/rest/1.0/transactions";
        public List<Transaction> GetTransactionsByPayerIds(string[] id, DateTime startDate)
        {
            var strStartdate = startDate.ToString("dd-MM-yyyy");
            var request = new TransactionRequest()
            {
                startdate = strStartdate,
                payers_edrpous = id 
            };

            var response= this.PostJson(baseUrl, request);

            var obj = JObject.Parse(response);
            var result = (JArray)(obj["response"]["transactions"]);

            var list = result.Select(p => JsonConvert.DeserializeObject<Transaction>(p.ToString())).ToList();
            return list;
        }
    }
}
