using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Customer:BaseEntity
    {
        [Key]
        public int CustomerHeaderId { get; set; }
        public string CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string EmalAddress { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
