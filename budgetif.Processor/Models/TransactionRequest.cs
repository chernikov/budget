using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Processor.Models
{
    class TransactionRequest
    {
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string[] regions { get; set; }
        public string[] payers_edrpous { get; set; }
        public string[] recipt_edrpous { get; set; }
    }
}

