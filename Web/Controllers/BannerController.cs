using Entities.Models;
using Repository;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class BannerController : Controller
    {
        private ProjectDbContext db = new ProjectDbContext();
        // GET: Banner
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(VmBanner model)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                #region Image
                var uri = Request.Url.Host;
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/Banner/" + uri));
                string path = "";
                if (model.BannerFile != null)
                {
                    string pic = System.IO.Path.GetFileName(model.BannerFile.FileName);
                    string physicalPath =
                        System.IO.Path.Combine(Server.MapPath("~/Images/Banner/" + uri), pic);
                    path = "/Images/Banner/" + uri + "/" + pic;
                    model.BannerFile.SaveAs(physicalPath);
                    model.BannerImgUrl = path;
                }
                #endregion
                var newdata = new Banner();
                    newdata.BannerHeaderId = model.BannerHeaderId;
                    newdata.BannerTitle = model.BannerTitle;
                    newdata.BannerDescription = model.BannerDescription;
                    newdata.BannerImgUrl = model.BannerImgUrl;
                    newdata.BannerTypeId = (int)BannerType.HomeSlider;
                    db.Banners.Add(newdata);
                    db.SaveChanges();
                    TempData["SuccessMsg"] = "Banner Added Successfully";
                    return RedirectToAction("Index");
            }
            else
            {
                TempData["FailedMsg"] = "Failed to Add Banner";
            }
            return View(model);
        }

        public ActionResult AjaxBanerList()
        {
            List<VmBanner> results = db.Banners.Where(a => a.Attribute5 != "false").Select(s => new VmBanner
            {
                BannerHeaderId = s.BannerHeaderId,
                BannerImgUrl = s.BannerImgUrl,
                BannerTitle = s.BannerTitle,
                BannerDescription = s.BannerDescription,
                BannerTypeId=s.BannerTypeId

            }).ToList();
            return Json(new
            {
                iTotalRecords = results.Count(),
                iTotalDisplayRecords = db.Banners.Count(),
                data = results
            },
            JsonRequestBehavior.AllowGet);

        }
    }
}