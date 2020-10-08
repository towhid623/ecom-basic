using Microsoft.AspNet.Identity;
using Repository;
using Service.ViewModels;
using System.Linq;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = User.Identity.GetUserId<int>();
            var appDbContext = new ApplicationDbContext();
            ViewBag.LoggedInUser = appDbContext.Users.Find(userId) !=null && appDbContext.Users.Find(userId).UserType == (int)UserType.Customer? appDbContext.Users.Find(userId):null;
            var db = new ProjectDbContext();
            var company = db.Company.FirstOrDefault();
            ViewBag.CompanyName = company != null && company.CompanyName != null ? company.CompanyName : "Company Name";
            ViewBag.CompanyEmail = company != null && company.EmailAddress != null ? company.EmailAddress : "Company Name";
            ViewBag.CompanyAddress = company != null && company.Address != null ? company.Address : "";
            ViewBag.Logo = company != null && company.ImageUrl != null ? company.ImageUrl : "";
            ViewBag.FacebookUrl = company != null && company.FacebookUrl != null ? company.FacebookUrl : "";
            ViewBag.InstagramUrl = company != null && company.InstagramUrl != null ? company.InstagramUrl : "";
            ViewBag.TwitterUrl = company != null && company.TwitterUrl != null ? company.TwitterUrl : "";
            ViewBag.CategoryList = db.ProductCategory.AsEnumerable();
            ViewBag.ProductList = db.Product.AsEnumerable().Select(s=>new VmProductList { ProductName=s.ProductName,ProductHeaderId=s.ProductHeaderId,Price=s.Rate.ToString("N2").Replace(",","")}).ToList();
            ViewBag.CompanyMobileNumber = company != null && company.MobileNumber != null ? company.MobileNumber : "";
            ViewBag.RegisterdEmails = db.Customer.Select(s => s.EmalAddress).ToList();
            base.OnActionExecuting(filterContext);
        }
    }
}