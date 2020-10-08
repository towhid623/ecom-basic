using Entities.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Repository;
using Service;
using Service.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class OrderController : BaseController
    {
        private ProjectDbContext db;
        private ApplicationDbContext applicationDbContext;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ICustomerService customerService;
        public OrderController(ProjectDbContext db, ApplicationDbContext applicationDbContext, ICustomerService customerService)
        {
            this.db = db;
            this.applicationDbContext = applicationDbContext;
            this.customerService = customerService;
        }

        #region Essential Functions
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        #endregion
        // GET: Order
        [HttpPost]
        public ActionResult Add(VmOrderAdd model)
        {
            var loggedInUser = UserManager.FindById<ApplicationUser, int>(User.Identity.GetUserId<int>());
            var customer = new Customer();
            if ((model.Customer == null || model.Customer.CustomerName == null) && loggedInUser != null)
            {
                customer = db.Customer.Where(w => w.CustomerHeaderId == loggedInUser.ReferrenceId).FirstOrDefault();
            }
            else
            {
                var cus = new VmCustomerAdd
                {
                    Address = model.Customer.Address,
                    CustomerHeaderId = model.Customer.CustomerHeaderId,
                    CustomerName = model.Customer.CustomerName,
                    EmailAddress = model.Customer.EmailAddress,
                    MobileNumber = model.Customer.MobileNumber
                };
                int? id;
                customerService.Add(cus, out id);
                customer = db.Customer.Find(id);
                var user = new ApplicationUser { UserName = model.Customer.EmailAddress, Email = model.Customer.EmailAddress, UserType = (int)UserType.Customer, ReferrenceId = customer.CustomerHeaderId, CreationDate = DateTime.Now, LastUpdatedDate = DateTime.Now };
                var result = UserManager.Create(user, model.Customer.Password);
                if (result.Succeeded)
                {
                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                }
            }

            var order = new Order
            {
                CustomerHeaderId = customer.CustomerHeaderId,
                OrderDate = DateTime.Now,
                OrderHeaderId = 0,
                Status = (int)OrderStatus.Pending,
                OrderNo = GenerateOrderNo()
            };
            db.Order.Add(order);
            db.SaveChanges();
            foreach (var item in model.Purchases)
            {
                var orderItem = new OrderItem
                {
                    OrderHeaderId = order.OrderHeaderId,
                    OrderItemHeaderId = 0,
                    ProductHeaderId = item.ProductHeaderId,
                    Quantity = item.Quantity,
                    Rate = db.Product.Find(item.ProductHeaderId).Rate,
                    Total = db.Product.Find(item.ProductHeaderId).Rate * item.Quantity
                };
                db.OrderItem.Add(orderItem);
            }
            db.SaveChanges();
            return Json(customer.CustomerHeaderId, JsonRequestBehavior.AllowGet);
        }

        public string GenerateOrderNo()
        {
            var resultantID = "000001";
            var lastItem = db.Order.OrderByDescending(o => o.OrderHeaderId).FirstOrDefault();
            string lItemID = lastItem == null ? "" : lastItem.OrderNo == null ? "" : lastItem.OrderNo == "" ? "" : lastItem.OrderNo;
            if (!string.IsNullOrEmpty(lItemID))
            {
                int cnt = Convert.ToInt32(lItemID);
                resultantID = (cnt + 1).ToString("D6");
            }
            return resultantID;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult OrderList(int id)
        {
            var customer = db.Customer.Where(w => w.CustomerHeaderId == id).FirstOrDefault();
            ViewBag.OrderStatus = from OrderStatus e in Enum.GetValues(typeof(OrderStatus))
                                  select new VmSelectList { Id = (int)e, Name = e.ToString() };
            return View(customer);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var order = db.Order.Where(w => w.OrderHeaderId == id).FirstOrDefault();
            return View(order);
        }

        public ActionResult AjaxOrderList(VmJqueryDataTable param)
        {
            var allValues = db.Order.AsEnumerable();
            var result = allValues.Select(s => new
            {
                s.OrderHeaderId,
                s.OrderNo,
                CustomerName = s.Customer.CustomerName??"",
                Mobile = s.Customer.MobileNumber ?? "",
                Address = s.Customer.Address ?? "",
                Date = s.OrderDate.ToString("dd MMM yyyy"),
                Price = s.OrderItem.Select(ss => ss.Total).Any() ? s.OrderItem.Select(ss => ss.Total).Sum() : 0,
                Items = s.OrderItem.Select(ss => ss.OrderItemHeaderId).Any() ? string.Join(",", s.OrderItem.Select(ss => ss.Product.ProductName)) : "",
                Status = s.Status == (int)OrderStatus.Pending ? OrderStatus.Pending.ToString() : (s.Status == (int)OrderStatus.Confirmed ? OrderStatus.Confirmed.ToString() : (s.Status == (int)OrderStatus.Delivered ? OrderStatus.Delivered.ToString() : (s.Status == (int)OrderStatus.Completed ? OrderStatus.Completed.ToString() : OrderStatus.Pending.ToString()))),
            });
            return Json(new
            {
                sEcho = param.draw,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = result.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AdminDetails(int id)
        {
            var order = db.Order.Where(w => w.OrderHeaderId == id).FirstOrDefault();
            return View(order);
        }
    }
}