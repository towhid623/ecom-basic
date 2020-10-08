using Entities.Models;
using Repository;
using Service.ViewModels;

namespace Service
{
    public class CustomerService : ICustomerService
    {
        private ProjectDbContext db;
        public CustomerService(ProjectDbContext db)
        {
            this.db = db;
        }
        public int Add(VmCustomerAdd model, out int? id)
        {
            int isSaved = 0;
            if (model.CustomerHeaderId == 0)
            {
                var cus = new Customer
                {
                    CustomerHeaderId = 0,
                    CustomerName = model.CustomerName,
                    Address = model.Address,
                    EmalAddress = model.EmailAddress,
                    MobileNumber = model.MobileNumber
                };
                db.Customer.Add(cus);
                isSaved = db.SaveChanges();
                id = cus.CustomerHeaderId;

            }
            else
            {
                var cus = db.Customer.Find(model.CustomerHeaderId);
                cus.CustomerHeaderId = 0;
                cus.CustomerName = model.CustomerName;
                cus.Address = model.Address;
                cus.EmalAddress = model.EmailAddress;
                cus.MobileNumber = model.MobileNumber;
                id = cus.CustomerHeaderId;
                isSaved = db.SaveChanges();
            }

            return isSaved;
        }
    }
}
