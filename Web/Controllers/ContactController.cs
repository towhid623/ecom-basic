using Service.Helper;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult ContactPartial()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(VmContact model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GMailer.SendMail(model);
                }
                catch
                {
                    return Json(JsonRequestBehavior.AllowGet);
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}