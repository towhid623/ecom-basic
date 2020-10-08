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
    public class ProductSubCategoryController : Controller
    {
        // GET: ProductSubCategory
        private ProjectDbContext db = new ProjectDbContext();
        // GET: ProductCategory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.CategoryList = db.ProductCategory.Select(s => new VmSelectList { Id = s.ProductCategoryHeaderId, Name = s.ProductCategoryName });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(VmProductSubCategoryAdd model,HttpPostedFileBase file)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.ProductSubCategory.Any(a =>a.ProductCategoryHeaderId==model.ProductCategoryHeaderId&& a.ProductSubCategoryName == model.ProductSubCategoryName))
                    {
                        TempData["FailedMsg"] = "Sub-Category Already Exist";
                    }
                    else
                    {
                        #region Image Upload
                        var uri = Request.Url.Host;
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/ProductSubCategory/" + uri));
                        string path = "";
                        if (file != null)
                        {
                            string pic = System.IO.Path.GetFileName(file.FileName);
                            string physicalPath =
                                System.IO.Path.Combine(Server.MapPath("~/Images/ProductSubCategory/" + uri), pic);
                            path = "/Images/ProductSubCategory/" + uri + "/" + pic;
                            file.SaveAs(physicalPath);
                            model.ImageUrl = path;
                        }
                        #endregion
                        var newData = new ProductSubCategory {
                            ProductCategoryHeaderId = model.ProductCategoryHeaderId,
                            ProductSubCategoryName = model.ProductSubCategoryName,
                            ImageUrl = model.ImageUrl,
                            ProductSubCategoryHeaderId = 0
                        };
                        db.ProductSubCategory.Add(newData);
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Sub-Category Added Successfully";
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
            ViewBag.CategoryList = db.ProductCategory.Select(s => new VmSelectList { Id = s.ProductCategoryHeaderId, Name = s.ProductCategoryName });
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (id > 0)
            {
                var data = db.ProductSubCategory.Find(id);
                if (data == null)
                {
                    TempData["FailedMsg"] = "Sub-Category Not Found";
                }
                else
                {
                    var model = new VmProductSubCategoryAdd
                    {
                        ProductCategoryHeaderId = data.ProductCategoryHeaderId,
                        ProductSubCategoryName = data.ProductSubCategoryName,
                        ProductSubCategoryHeaderId = data.ProductSubCategoryHeaderId
                    };
                    ViewBag.CategoryList = db.ProductCategory.Select(s => new VmSelectList { Id = s.ProductCategoryHeaderId, Name = s.ProductCategoryName });
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
        public ActionResult Edit(VmProductSubCategoryAdd model,HttpPostedFileBase file)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db.ProductSubCategory.Find(model.ProductSubCategoryHeaderId);
                    if (data == null)
                    {
                        TempData["FailedMsg"] = "Sub-Category Not Found";
                    }
                    else
                    {
                        #region Image Upload
                        var uri = Request.Url.Host;
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/ProductSubCategory/" + uri));
                        string path = "";
                        if (file != null)
                        {
                            string pic = System.IO.Path.GetFileName(file.FileName);
                            string physicalPath =
                                System.IO.Path.Combine(Server.MapPath("~/Images/ProductSubCategory/" + uri), pic);
                            path = "/Images/ProductSubCategory/" + uri + "/" + pic;
                            file.SaveAs(physicalPath);
                            data.ImageUrl = path;
                        }
                        #endregion
                        data.ProductSubCategoryName = model.ProductSubCategoryName;
                        data.ProductCategoryHeaderId = model.ProductCategoryHeaderId;
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Sub-Category Updated Successfully";
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
            ViewBag.CategoryList = db.ProductCategory.Select(s => new VmSelectList { Id = s.ProductCategoryHeaderId, Name = s.ProductCategoryName });
            return View(model);
        }

        public ActionResult AjaxSubCategoryList(VmJqueryDataTable param)
        {
            int totalLength = 0;
            string searchValue = Request.QueryString["search[value]"];
            string columnIndex = Request.QueryString["order[0][column]"];
            string orderDirection = Request.QueryString["order[0][dir]"];
            var allValues = db.ProductSubCategory.AsEnumerable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                allValues = allValues.Where(w => w.ProductSubCategoryName.ToLower().Contains(searchValue.ToLower())|| w.ProductCategory.ProductCategoryName.ToLower().Contains(searchValue.ToLower()));
            }
            var result = allValues.Select(s => new
            {
                ProductSubCategoryHeaderId=s.ProductSubCategoryHeaderId,
                ProductCategoryHeaderId = s.ProductCategoryHeaderId,
                ProductCategoryName = s.ProductCategory.ProductCategoryName,
                ProductSubCategoryName = s.ProductSubCategoryName,
                ImageUrl = s.ImageUrl,
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