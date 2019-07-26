using budgetif.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Processor
{
    public class ProcessVotesProcessor : BaseProcessor
    {
        private BudgetIfContext _db;

        public ProcessVotesProcessor()
        {
            _db = new BudgetIfContext("Server=tcp:budgetifsql.database.windows.net,1433;Database=budgetif;User ID=chernikov;Password=Million30;Encrypt=True;TrustServerCertificate=False;Connection Timeout=200;");
        }

        public override void Run(object input = null)
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"DataUpdate_budgetif_2.sql");

            
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("INSERT"))
                {
                    try
                    {
                        //_db.Database.SqlQuery(typeof(bool), line);
                        _db.Database.ExecuteSqlCommand(line);
                        counter++;
                    } catch (Exception ex)
                    {

                    }
                }
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            System.Console.ReadLine();
            throw new NotImplementedException();
        }
    }
}
