using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MT.WCF.User;
using MT.WCF.User.Request;

namespace MT.Test.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var client = new UserService.UserServiceClient();
            var getuer = client.GetUser(new ParamForGetUser() {UserId = "1"});
            return View();
        }
    }
}