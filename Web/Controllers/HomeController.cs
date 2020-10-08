using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private ProjectDbContext db = new ProjectDbContext();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.HomeSlider = db.Banners.OrderBy(a => a.BannerTypeId == 1);
            ViewBag.NewProducts = db.Product.OrderByDescending(o => o.ProductHeaderId);
            ViewBag.PopularProducts = db.Product.OrderBy(o => o.ProductHeaderId);
            return View();
        }

        public ActionResult ProductCategory(int id)
        {
            var data = db.ProductCategory.Where(w => w.ProductCategoryHeaderId == id).FirstOrDefault();
            return View(data);
        }

        public ActionResult Product(int id)
        {
            var data = db.Product.Where(w => w.ProductHeaderId == id).FirstOrDefault();
            return View(data);
        }

        public ActionResult OrderConfirm(int id)
        {
            var data = db.Order.Where(w => w.OrderHeaderId == id).FirstOrDefault();
            return View(data);
        }
    }
}