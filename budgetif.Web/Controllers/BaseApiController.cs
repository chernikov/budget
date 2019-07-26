using budgetif.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace budgetif.Web.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected IBudgetIfContext Context;

        public BaseApiController()
        {
            Context = DependencyResolver.Current.GetService<IBudgetIfContext>();
        }

    }
}
