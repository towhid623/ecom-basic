using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service.ViewModels
{
    public class VmCompanyAdd
    {
        public int? CompanyHeaderId { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Mobile Number is required")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumberOptional { get; set; }
        public string SignatureUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public HttpPostedFileBase Logo { get; set; }
        public HttpPostedFileBase Signature { get; set; }
    }
}
