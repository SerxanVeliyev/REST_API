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
        Operation_C Operations = new Operation_C();//Operator takip
        public string GET(string User, string Password, int Page, string _operator, string expeditor, string name, DateTime startdate, DateTime enddate)
        {
            if (Operations.Security(User, Password))
                return Operations.Operation(Page, _operator, expeditor, name, startdate, enddate);
            else
                return "Username or password is incorrect";
        }
        public string GET(string User, string Password, int page, string _operator, string orderstatus, DateTime startdate, DateTime enddate)//Operator Sifarisi
        {
            if (Operations.Security(User, Password))
                return Operations.Operation_order(page, _operator, orderstatus, startdate, enddate);
            else
                return "Username or password is incorrect";
        }
        public string GET(string User, string Password, int page, string _operator, DateTime startdate, DateTime enddate)//Mehsul Satisi
        {
            if (Operations.Security(User, Password))
                return Operations.Product_sell(page, _operator, startdate, enddate);
            else
                return "Username or password is incorrect";
        }
        public string GET(string User, string Password)//Operator Sifarisi dashboard filter
        {
            if (Operations.Security(User, Password))
                return Operations.OperatorName_and_OrderStatus();
            else
                return "Username or password is incorrect";
        }
        public string GET(string User, string Password,string OperatorName, string OrderStatus, DateTime startDate, DateTime endDate)
        {
            if (Operations.Security(User, Password))
                return Operations.OperatorName_and_OrderStatus_Dashboard(OperatorName, OrderStatus, startDate, endDate);
            else
                return "Username or password is incorrect";
        }
    }
}
