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
    public class ProductController : Controller
    {
        // GET: Product
        private ProjectDbContext db = new ProjectDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.SubCategoryList = db.ProductSubCategory.AsEnumerable().Select(s=>new ProductSubCategory { ProductCategoryHeaderId = s.ProductCategoryHeaderId,ProductSubCategoryHeaderId=s.ProductSubCategoryHeaderId,ProductSubCategoryName=s.ProductSubCategoryName});
            ViewBag.UnitList = db.FndFlexValueSet.Where(w=>w.FlexValueSetShortName=="unit").FirstOrDefault().FndFlexValue.Select(s=>new VmSelectList { Id=s.FlexValueId,Name=s.FlexValue});
            ViewBag.BrandList = db.FndFlexValueSet.Where(w=>w.FlexValueSetShortName=="brand").FirstOrDefault().FndFlexValue.Select(s=>new VmSelectList { Id=s.FlexValueId,Name=s.FlexValue});
            ViewBag.CategoryList = db.ProductCategory.Select(s=>new VmSelectList { Id=s.ProductCategoryHeaderId,Name=s.ProductCategoryName});
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(VmProductAdd model)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Product.Any(a => a.ProductName == model.ProductName))
                    {
                        TempData["FailedMsg"] = "Product Already Exist";
                    }
                    else if (db.Product.Any(a => a.ProductCode == model.ProductCode))
                    {
                        TempData["FailedMsg"] = "Product Code Already Exist";
                    }
                    else
                    {
                        #region Image Upload
                        var uri = Request.Url.Host;
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/Product/" + uri));
                        string path = "";
                        if (model.Image1 != null)
                        {
                            string pic = System.IO.Path.GetFileName(model.Image1.FileName);
                            string physicalPath =
                                System.IO.Path.Combine(Server.MapPath("~/Images/Product/" + uri), pic);
                            path = "/Images/Product/" + uri + "/" + pic;
                            model.Image1.SaveAs(physicalPath);
                            model.ImageUrl1 = path;
                        }
                        path = "";
                        if (model.Image2 != null)
                        {
                            string pic = System.IO.Path.GetFileName(model.Image2.FileName);
                            string physicalPath =
                                System.IO.Path.Combine(Server.MapPath("~/Images/Product/" + uri), pic);
                            path = "/Images/Product/" + uri + "/" + pic;
                            model.Image2.SaveAs(physicalPath);
                            model.ImageUrl2 = path;
                        }
                        path = "";
                        if (model.Image3 != null)
                        {
                            string pic = System.IO.Path.GetFileName(model.Image3.FileName);
                            string physicalPath =
                                System.IO.Path.Combine(Server.MapPath("~/Images/Product/" + uri), pic);
                            path = "/Images/Product/" + uri + "/" + pic;
                            model.Image3.SaveAs(physicalPath);
                            model.ImageUrl3 = path;
                        }
                        #endregion
                        var newData = new Product();
                        newData.BrandId = model.BrandId;
                        newData.Description = model.Description;
                        newData.ModelNo = model.ModelNo;
                        newData.ProductCode = model.ProductCode;
                        newData.ProductHeaderId = model.ProductHeaderId;
                        newData.ProductName = model.ProductName;
                        newData.ProductSubCategoryHeaderId = model.ProductSubCategoryHeaderId;
                        newData.Rate = model.Rate;
                        newData.UnitId = model.UnitId;
                        newData.ImageUrl3 = model.ImageUrl3;
                        newData.ImageUrl2 = model.ImageUrl2;
                        newData.ImageUrl1 = model.ImageUrl1;
                        db.Product.Add(newData);
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Product Added Successfully";
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
            ViewBag.SubCategoryList = db.ProductSubCategory.AsEnumerable().Select(s => new ProductSubCategory { ProductCategoryHeaderId = s.ProductCategoryHeaderId, ProductSubCategoryHeaderId = s.ProductSubCategoryHeaderId, ProductSubCategoryName = s.ProductSubCategoryName });
            ViewBag.UnitList = db.FndFlexValueSet.Where(w => w.FlexValueSetShortName == "unit").FirstOrDefault().FndFlexValue.Select(s => new VmSelectList { Id = s.FlexValueId, Name = s.FlexValue });
            ViewBag.BrandList = db.FndFlexValueSet.Where(w => w.FlexValueSetShortName == "brand").FirstOrDefault().FndFlexValue.Select(s => new VmSelectList { Id = s.FlexValueId, Name = s.FlexValue });
            ViewBag.CategoryList = db.ProductCategory.Select(s => new VmSelectList { Id = s.ProductCategoryHeaderId, Name = s.ProductCategoryName });
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (id > 0)
            {
                var data = db.Product.Where(w=>w.ProductHeaderId==id).FirstOrDefault();
                if (data == null)
                {
                    TempData["FailedMsg"] = "Product Not Found";
                }
                else
                {
                    var model = new VmProductAdd
                    {
                        BrandId = data.BrandId,
                        Description = data.Description,
                        ModelNo = data.ModelNo,
                        ProductCategoryHeaderId = data.ProductSubCategory.ProductCategory.ProductCategoryHeaderId,
                        ProductCode = data.ProductCode,
                        ProductHeaderId = data.ProductHeaderId,
                        ProductName = data.ProductName,
                        ProductSubCategoryHeaderId = data.ProductSubCategoryHeaderId,
                        Rate = data.Rate,
                        UnitId = data.UnitId,
                    };
                    ViewBag.SubCategoryList = db.ProductSubCategory.AsEnumerable().Select(s => new ProductSubCategory { ProductCategoryHeaderId = s.ProductCategoryHeaderId, ProductSubCategoryHeaderId = s.ProductSubCategoryHeaderId, ProductSubCategoryName = s.ProductSubCategoryName });
                    ViewBag.UnitList = db.FndFlexValueSet.Where(w => w.FlexValueSetShortName == "unit").FirstOrDefault().FndFlexValue.Select(s => new VmSelectList { Id = s.FlexValueId, Name = s.FlexValue });
                    ViewBag.BrandList = db.FndFlexValueSet.Where(w => w.FlexValueSetShortName == "brand").FirstOrDefault().FndFlexValue.Select(s => new VmSelectList { Id = s.FlexValueId, Name = s.FlexValue });
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
        public ActionResult Edit(VmProductAdd model)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db.Product.Find(model.ProductHeaderId);
                    if (data == null)
                    {
                        TempData["FailedMsg"] = "Product Not Found";
                    }
                    else
                    {
                        #region Image Upload
                        var uri = Request.Url.Host;
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/Product/" + uri));
                        string path = "";
                        if (model.Image1 != null)
                        {
                            string pic = System.IO.Path.GetFileName(model.Image1.FileName);
                            string physicalPath =
                                System.IO.Path.Combine(Server.MapPath("~/Images/Product/" + uri), pic);
                            path = "/Images/Product/" + uri + "/" + pic;
                            model.Image1.SaveAs(physicalPath);
                            data.ImageUrl1 = path;
                        }
                        path = "";
                        if (model.Image2 != null)
                        {
                            string pic = System.IO.Path.GetFileName(model.Image2.FileName);
                            string physicalPath =
                                System.IO.Path.Combine(Server.MapPath("~/Images/Product/" + uri), pic);
                            path = "/Images/Product/" + uri + "/" + pic;
                            model.Image2.SaveAs(physicalPath);
                            data.ImageUrl2 = path;
                        }
                        path = "";
                        if (model.Image3 != null)
                        {
                            string pic = System.IO.Path.GetFileName(model.Image3.FileName);
                            string physicalPath =
                                System.IO.Path.Combine(Server.MapPath("~/Images/Product/" + uri), pic);
                            path = "/Images/Product/" + uri + "/" + pic;
                            model.Image3.SaveAs(physicalPath);
                            data.ImageUrl3 = path;
                        }
                        #endregion
                        data.BrandId = model.BrandId;
                        data.Description = model.Description;
                        data.ModelNo = model.ModelNo;
                        data.ProductCode = model.ProductCode;
                        data.ProductName = model.ProductName;
                        data.ProductSubCategoryHeaderId = model.ProductSubCategoryHeaderId;
                        data.Rate = model.Rate;
                        data.UnitId = model.UnitId;
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Product Updated Successfully";
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

        public ActionResult AjaxProductList(VmJqueryDataTable param)
        {
            int totalLength = 0;
            string searchValue = Request.QueryString["search[value]"];
            string columnIndex = Request.QueryString["order[0][column]"];
            string orderDirection = Request.QueryString["order[0][dir]"];
            var flexList = db.FndFlexValue.AsEnumerable();
            var allValues = db.Product.ToList();
            if (!string.IsNullOrEmpty(searchValue))
            {
                allValues = allValues.Where(w => w.ProductName.ToLower().Contains(searchValue.ToLower())|| w.ProductCode.ToLower().Contains(searchValue.ToLower())|| w.Rate.ToString().ToLower().Contains(searchValue.ToLower())|| (!string.IsNullOrWhiteSpace(w.Description)&&w.Description.ToLower().Contains(searchValue.ToLower()))).ToList();
            }
            var result = allValues.Select(s => new VmProductList{
                ProductHeaderId = s.ProductHeaderId,
                ProductCategoryName = s.ProductSubCategory.ProductCategory.ProductCategoryName,
                ProductSubCategoryName = s.ProductSubCategory.ProductSubCategoryName,
                ProductCode = s.ProductCode,
                ProductName = s.ProductName,
                ImageUrl = s.ImageUrl1,
                BrandName = flexList.FirstOrDefault(f=>f.FlexValueId==s.BrandId).FlexValue,
                UnitName = flexList.FirstOrDefault(f=>f.FlexValueId==s.UnitId).FlexValue,
                Price = s.Rate.ToString("N2"),
                Description = !string.IsNullOrEmpty(s.Description)&&s.Description.Length>20?s.Description.Substring(0,20):s.Description,
            });
            totalLength = result.Select(s => s.ProductHeaderId).Count();
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
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }
    }
}