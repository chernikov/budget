using AutoMapper;
using budgetif.Model;
using budgetif.Model.Enitities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Processor
{
    public class TransactionProcessor : BaseProcessor
    {
        private IBudgetIfContext _db;

        private List<Payer> Payers { get; set; }

        private List<Receipt> Receipts { get; set; }

        private IMapper _mapper;
        public TransactionProcessor(IBudgetIfContext db)
        {
            _db = db;
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Models.Transaction, Transaction>()
                .ForMember(p => p.amount, opt => opt.MapFrom(r => Convert.ToDouble(r.amount)))
                    .ForMember(p => p.doc_date, opt => opt.MapFrom(r => new DateTime(Convert.ToInt32(r.doc_date.Substring(6, 4)), Convert.ToInt32(r.doc_date.Substring(3, 2)), Convert.ToInt32(r.doc_date.Substring(0, 2)))))
                    .ForMember(p => p.doc_v_date, opt => opt.MapFrom(r => new DateTime(Convert.ToInt32(r.doc_v_date.Substring(6, 4)), Convert.ToInt32(r.doc_v_date.Substring(3, 2)), Convert.ToInt32(r.doc_v_date.Substring(0, 2)))))
                    .ForMember(p => p.trans_date, opt => opt.MapFrom(r => new DateTime(Convert.ToInt32(r.trans_date.Substring(6, 4)), Convert.ToInt32(r.doc_v_date.Substring(3, 2)), Convert.ToInt32(r.trans_date.Substring(0, 2)))))
                    .ForMember(p => p.number, opt => opt.MapFrom(r => r.id))
                    .ForMember(p => p.id, opt => opt.Ignore());
            });

            _mapper = config.CreateMapper();
        }

        public override void Run(object input)
        {
            if (input is string)
            {
                var str = (string)input;
                var transactionConnector = new TransactionConnector();
                Payers = new List<Payer>();
                Receipts = new List<Receipt>();
                var numbers = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                var ids = numbers.ToArray();
                var lastDate = _db.Transactions.OrderByDescending(p => p.trans_date).FirstOrDefault()?.trans_date;

                var list = transactionConnector.GetTransactionsByPayerIds(ids, lastDate ?? new DateTime(2017, 7, 1));

                Console.WriteLine($"Total entities {list.Count}");
                foreach (var item in list)
                {
                    var entity = _mapper.Map<Transaction>(item);
                    Payer payer;
                    if (item.payer_edrpou != "xxxxxxxxxx")
                    {
                        payer = Payers.FirstOrDefault(p => p.payer_edrpou == item.payer_edrpou);
                    } else
                    {
                        payer = Payers.FirstOrDefault(p => p.payer_edrpou == item.payer_edrpou && p.payer_name == item.payer_name);
                    }
                    if (payer == null)
                    {
                        payer = new Payer()
                        {
                            payer_edrpou = item.payer_edrpou,
                            payer_name = item.payer_name
                        };
                        _db.Payers.Add(payer);
                        _db.SaveChanges();
                        Payers.Add(payer);
                    }
                    entity.payerId = payer.id;
                    Receipt receipt;
                    if (item.recipt_edrpou != "xxxxxxxxxx")
                    {
                        receipt = Receipts.FirstOrDefault(p => p.recipt_edrpou == item.recipt_edrpou);
                    }
                    else
                    {
                        receipt = Receipts.FirstOrDefault(p => p.recipt_edrpou == item.recipt_edrpou && p.recipt_name == item.recipt_name);
                    }
                    if (receipt == null)
                    {
                        receipt = new Receipt()
                        {
                            recipt_edrpou = item.recipt_edrpou,
                            recipt_name = item.recipt_name
                        };
                        _db.Receipts.Add(receipt);
                        _db.SaveChanges();
                        Receipts.Add(receipt);
                    }
                    entity.receiptId = receipt.id;

                    _db.Transactions.Add(entity);
                    Console.WriteLine($"Save {entity.trans_date}");
                    _db.SaveChanges();
                }
            }
        }
    }
}
