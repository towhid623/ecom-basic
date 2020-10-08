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
    public class BrandController : Controller
    {
        // GET: Brand
        private ProjectDbContext db = new ProjectDbContext();
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
        public ActionResult Add(VmBrandAdd model)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.FndFlexValueSet.Where(w => w.FlexValueSetShortName == "brand").FirstOrDefault().FndFlexValue.Any(a => a.FlexValue == model.BrandName))
                    {
                        TempData["FailedMsg"] = "Brand Already Exist";
                    }
                    else
                    {
                        var newData = new FndFlexValue { FlexValueId = model.BrandHeaderId, FlexValue = model.BrandName, FlexValueSetId = db.FndFlexValueSet.FirstOrDefault(w => w.FlexValueSetShortName == "brand").FlexValueSetId };
                        db.FndFlexValue.Add(newData);
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Brand Added Successfully";
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
                var model = db.FndFlexValue.Find(id);
                if (model == null)
                {
                    TempData["FailedMsg"] = "Brand Not Found";
                }
                else
                {
                    var data = new VmBrandAdd { BrandHeaderId = model.FlexValueId, BrandName = model.FlexValue };
                    return View(data);
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
        public ActionResult Edit(VmBrandAdd model)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db.FndFlexValue.Find(model.BrandHeaderId);
                    if (data == null)
                    {
                        TempData["FailedMsg"] = "Brand Not Found";
                    }
                    else
                    {
                        data.FlexValue = model.BrandName;
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Brand Updated Successfully";
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

        public ActionResult AjaxBrandList(VmJqueryDataTable param)
        {
            int totalLength = 0;
            string searchValue = Request.QueryString["search[value]"];
            string columnIndex = Request.QueryString["order[0][column]"];
            string orderDirection = Request.QueryString["order[0][dir]"];
            var allValues = db.FndFlexValueSet.Where(w => w.FlexValueSetShortName == "brand").FirstOrDefault().FndFlexValue.AsEnumerable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                allValues = allValues.Where(w => w.FlexValue.ToLower().Contains(searchValue.ToLower()));
            }
            var result = allValues.Select(s => new {
                BrandHeaderId = s.FlexValueId,
                BrandName = s.FlexValue
            });
            totalLength = result.Select(s => s.BrandHeaderId).Count();
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