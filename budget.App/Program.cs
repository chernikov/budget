using budgetif.Model;
using budgetif.Processor;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budget.App
{
    class Program
    {
        static IContainer container;
        static void Main(string[] args)
        {
            InitContainer();

            var processor = container.GetInstance<BaseProcessor>();
            var str = string.Empty;
            using (var sr = new StreamReader("numbers.txt"))
            {
                str = sr.ReadToEnd();
            }
            var list = str.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //var fileName = "edr.xml";
            processor.Run(list);
            Console.ReadLine();
        }

        static void InitContainer()
        {
            container = new Container();
            container.Configure(cfg => cfg.For<IBudgetIfContext>().Use<BudgetIfContext>());
            container.Configure(cfg => cfg.For<BaseProcessor>().Use<ProcessVotesProcessor>());
        }
    }
}
