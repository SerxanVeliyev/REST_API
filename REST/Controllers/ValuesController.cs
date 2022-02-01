using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace REST.Controllers
{
    public class ValuesController : ApiController
    {
        Operation_C Operations = new Operation_C();
        public string GET(string User, string Password, int Page)
        {
            if (Operations.Security(User, Password))
                return Operations.Operation(Page);
            else
                return "Username or password is incorrect";
        }
    }
}
