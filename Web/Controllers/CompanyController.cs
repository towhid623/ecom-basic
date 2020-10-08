using Entities.Models;
using Repository;
using Service.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        // GET: Company
        private ProjectDbContext db = new ProjectDbContext();
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            VmCompanyAdd model = new VmCompanyAdd();
            var oldData = db.Company.FirstOrDefault();
            if (oldData != null)
            {
                model.Address = oldData.Address;
                model.CompanyHeaderId = oldData.CompanyHeaderId;
                model.CompanyName = oldData.CompanyName;
                model.EmailAddress = oldData.EmailAddress;
                model.FacebookUrl = oldData.FacebookUrl;
                model.InstagramUrl = oldData.InstagramUrl;
                model.MobileNumber = oldData.MobileNumber;
                model.MobileNumberOptional = oldData.MobileNumberOptional;
                model.WebsiteUrl = oldData.WebsiteUrl;
                model.TwitterUrl = oldData.TwitterUrl;
                model.PhoneNumber = oldData.PhoneNumber;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(VmCompanyAdd model)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    #region Image Upload
                    var uri = Request.Url.Host;
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/Company/" + uri));
                    string path = "";
                    if (model.Logo != null)
                    {
                        string pic = System.IO.Path.GetFileName(model.Logo.FileName);
                        string physicalPath =
                            System.IO.Path.Combine(Server.MapPath("~/Images/Company/" + uri), pic);
                        path = "/Images/Company/" + uri + "/" + pic;
                        model.Logo.SaveAs(physicalPath);
                        model.ImageUrl = path;
                    }
                    path = "";
                    if (model.Signature != null)
                    {
                        string pic = System.IO.Path.GetFileName(model.Signature.FileName);
                        string physicalPath =
                            System.IO.Path.Combine(Server.MapPath("~/Images/Company/" + uri), pic);
                        path = "/Images/Company/" + uri + "/" + pic;
                        model.Signature.SaveAs(physicalPath);
                        model.SignatureUrl = path;
                    }
                    #endregion
                    if (model.CompanyHeaderId>0&&db.Company.Any(a=>a.CompanyHeaderId==model.CompanyHeaderId))
                    {
                        var oldData = db.Company.Find(model.CompanyHeaderId);
                        oldData.Address = model.Address;
                        oldData.CompanyName = model.CompanyName;
                        oldData.EmailAddress = model.EmailAddress;
                        oldData.FacebookUrl = model.FacebookUrl;
                        oldData.InstagramUrl = model.InstagramUrl;
                        oldData.MobileNumber = model.MobileNumber;
                        oldData.MobileNumberOptional = model.MobileNumberOptional;
                        oldData.WebsiteUrl = model.WebsiteUrl;
                        oldData.TwitterUrl = model.TwitterUrl;
                        oldData.PhoneNumber = model.PhoneNumber;
                        if (!string.IsNullOrEmpty(model.ImageUrl))
                        {
                            oldData.ImageUrl = model.ImageUrl;
                            oldData.SignatureUrl = model.SignatureUrl;
                        }
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Company Information Updated Successfully";
                    }
                    else
                    {
                        var newData = new Company {
                            Address = model.Address,
                            CompanyHeaderId = 0,
                            CompanyName = model.CompanyName,
                            EmailAddress = model.EmailAddress,
                            FacebookUrl = model.FacebookUrl,
                            InstagramUrl = model.InstagramUrl,
                            MobileNumber = model.MobileNumber,
                            MobileNumberOptional = model.MobileNumberOptional,
                            WebsiteUrl = model.WebsiteUrl,
                            TwitterUrl = model.TwitterUrl,
                            PhoneNumber = model.PhoneNumber
                        };
                        if (!string.IsNullOrEmpty(model.ImageUrl))
                        {
                            newData.ImageUrl = model.ImageUrl;
                            newData.SignatureUrl = model.SignatureUrl;
                        }
                        db.Company.Add(newData);
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Company Information Added Successfully";
                    }
                }
                catch (Exception e)
                {
                    TempData["FailedMsg"] = e.Message;
                }
            }
            else
            {
                TempData["FailedMsg"] = "Failed";
            }
            return View(model);
        }
    }
}