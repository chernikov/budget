using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Processor
{
    public class EdrProcessor : BaseProcessor
    {
        static int blockSize = 1024;
        public override void Run(object input = null)
        {
            var fileName = (string)input;

            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        var sb = new StringBuilder();

                        var buffer = new char[blockSize];

                        var offset = 0;

                        while (true)
                        {
                            var size = sr.Read(buffer, offset, blockSize);
                            if (size != 0)
                            {
                                return;
                            }
                            sb.Append(buffer);
                            offset += blockSize;

                            var data = sb.ToString();
                            if (data.Contains("<RECORD>") && data.Contains("</RECORD>"))
                            {
                                data = ProcessRecords(data);
                            }
                            if (size < blockSize)
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }

        private string ProcessRecords(string data)
        {
            throw new NotImplementedException();
        }
    }
}
