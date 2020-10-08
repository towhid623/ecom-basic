using Entities.Models;
using Repository;
using Service.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class ProductCategoryController : Controller
    {
        private ProjectDbContext db = new ProjectDbContext();
        // GET: ProductCategory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model,HttpPostedFileBase file)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.ProductCategory.Any(a => a.ProductCategoryName == model.ProductCategoryName))
                    {
                        TempData["FailedMsg"] = "Category Already Exist";
                    }
                    else
                    {
                        #region Image Upload
                        var uri = Request.Url.Host;
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/ProductCategory/" + uri));
                        string path = "";
                        if (file != null)
                        {
                            string pic = System.IO.Path.GetFileName(file.FileName);
                            string physicalPath =
                                System.IO.Path.Combine(Server.MapPath("~/Images/ProductCategory/" + uri), pic);
                            path = "/Images/ProductCategory/" + uri + "/" + pic;
                            file.SaveAs(physicalPath);
                            model.ImageUrl = path;
                        }
                        #endregion
                        db.ProductCategory.Add(model);
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Category Added Successfully";
                        return RedirectToAction("Index");
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

        public ActionResult Edit(int id)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (id > 0)
            {
                var model = db.ProductCategory.Find(id);
                if (model == null)
                {
                    TempData["FailedMsg"] = "Category Not Found";
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                TempData["FailedMsg"] = "Invalid Url";
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model,HttpPostedFileBase file)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db.ProductCategory.Find(model.ProductCategoryHeaderId);
                    if(data == null)
                    {
                        TempData["FailedMsg"] = "Category Not Found";
                    }
                    else
                    {
                        #region Image Upload
                        var uri = Request.Url.Host;
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/ProductCategory/" + uri));
                        string path = "";
                        if (file != null)
                        {
                            string pic = System.IO.Path.GetFileName(file.FileName);
                            string physicalPath =
                                System.IO.Path.Combine(Server.MapPath("~/Images/ProductCategory/" + uri), pic);
                            path = "/Images/ProductCategory/" + uri + "/" + pic;
                            file.SaveAs(physicalPath);
                            data.ImageUrl = path;
                        }
                        #endregion
                        data.ProductCategoryName = model.ProductCategoryName;
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Category Updated Successfully";
                        return RedirectToAction("Index");
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

        public ActionResult AjaxCategoryList(VmJqueryDataTable param)
        {
            int totalLength = 0;
            string searchValue = Request.QueryString["search[value]"];
            string columnIndex = Request.QueryString["order[0][column]"];
            string orderDirection = Request.QueryString["order[0][dir]"];
            var allValues = db.ProductCategory.AsEnumerable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                allValues = allValues.Where(w => w.ProductCategoryName.ToLower().Contains(searchValue.ToLower()));
            }
            var result = allValues.Select(s => new {
                ProductCategoryHeaderId=s.ProductCategoryHeaderId,
                ProductCategoryName = s.ProductCategoryName,
                ImageUrl = s.ImageUrl
            });
            totalLength = result.Select(s => s.ProductCategoryHeaderId).Count();
            if (param.length == -1)
            {
                param.length = totalLength;
            }
            var displayedValues = result.Skip(param.start)
                 .Take(param.length).ToList();
            return Json(new
            {
                sEcho = param.draw,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = totalLength,
                aaData = displayedValues
            }, JsonRequestBehavior.AllowGet);
        }
    }
}