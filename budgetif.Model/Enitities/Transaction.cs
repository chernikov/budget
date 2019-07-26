using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Model.Enitities
{
    public class Transaction
    {
        public int id { get; set; }

        [Index(IsClustered = false, IsUnique = true, Order = 1)]
        public int number { get; set; }

        public string doc_number { get; set; }

        public DateTime doc_date { get; set; }

        public DateTime doc_v_date { get; set; }

        public DateTime trans_date { get; set; }

        public double amount { get; set; }
      
        public int payerId { get; set; }

        public virtual Payer payer { get; set; }

        public int receiptId { get; set; }

        public virtual Receipt receipt { get; set; }
     
        public string payment_details { get; set; }
        
    }

}
