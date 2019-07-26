using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Processor.Models
{
    public class Transaction
    {
        public int id { get; set; }

        public string doc_number { get; set; }
        public string doc_date { get; set; }
        public string doc_v_date { get; set; }
        public string trans_date { get; set; }
        public string amount { get; set; }
        public string payer_edrpou { get; set; }
        public string payer_name { get; set; }
        public string payer_account { get; set; }
        public string payer_mfo { get; set; }
        public string payer_bank { get; set; }
        public string recipt_edrpou { get; set; }
        public string recipt_name { get; set; }
        public string recipt_account { get; set; }
        public string recipt_mfo { get; set; }
        public string recipt_bank { get; set; }
        public string payment_details { get; set; }
        public object doc_add_attr { get; set; }
        public int region_id { get; set; }
    }
}
