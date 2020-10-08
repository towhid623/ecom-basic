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
    public class UnitController : Controller
    {
        // GET: Unit
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
        public ActionResult Add(VmUnitAdd model)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.FndFlexValueSet.Where(w=>w.FlexValueSetShortName=="unit").FirstOrDefault().FndFlexValue.Any(a=>a.FlexValue == model.UnitName))
                    {
                        TempData["FailedMsg"] = "Unit Already Exist";
                    }
                    else
                    {
                        var newData = new FndFlexValue { FlexValueId = model.UnitHeaderId, FlexValue = model.UnitName,FlexValueSetId= db.FndFlexValueSet.FirstOrDefault(w => w.FlexValueSetShortName == "unit").FlexValueSetId };
                        db.FndFlexValue.Add(newData);
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Unit Added Successfully";
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
                    TempData["FailedMsg"] = "Unit Not Found";
                }
                else
                {
                    var data = new VmUnitAdd { UnitHeaderId = model.FlexValueId, UnitName = model.FlexValue };
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
        public ActionResult Edit(VmUnitAdd model)
        {
            TempData["SuccessMsg"] = "";
            TempData["FailedMsg"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db.FndFlexValue.Find(model.UnitHeaderId);
                    if (data == null)
                    {
                        TempData["FailedMsg"] = "Unit Not Found";
                    }
                    else
                    {
                        data.FlexValue = model.UnitName;
                        db.SaveChanges();
                        TempData["SuccessMsg"] = "Unit Updated Successfully";
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

        public ActionResult AjaxUnitList(VmJqueryDataTable param)
        {
            int totalLength = 0;
            string searchValue = Request.QueryString["search[value]"];
            string columnIndex = Request.QueryString["order[0][column]"];
            string orderDirection = Request.QueryString["order[0][dir]"];
            var allValues = db.FndFlexValueSet.Where(w=>w.FlexValueSetShortName=="unit").FirstOrDefault().FndFlexValue.AsEnumerable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                allValues = allValues.Where(w => w.FlexValue.ToLower().Contains(searchValue.ToLower()));
            }
            var result = allValues.Select(s => new {
                UnitHeaderId = s.FlexValueId,
                UnitName = s.FlexValue
            });
            totalLength = result.Select(s => s.UnitHeaderId).Count();
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