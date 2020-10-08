using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Company : BaseEntity
    {
        [Key]
        public int CompanyHeaderId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
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
    }
}
