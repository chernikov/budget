using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Processor
{
    public abstract class BaseProcessor
    {
        public abstract void Run(object input = null);
    }
}
