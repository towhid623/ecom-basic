using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext appDbContext = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId<int>();
            if (userId > 0)
            {
                var user = appDbContext.Users.Find(userId);
                if(user.UserType == (int)UserType.Admin)
                {
                    return RedirectToAction("Add", "Company");
                }
            }
            return RedirectToAction("AdminLogin", "Account");
        }
    }
}