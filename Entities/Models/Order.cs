using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Order:BaseEntity
    {
        [Key]
        public int OrderHeaderId { get; set; }
        [Required]
        public string OrderNo { get; set; }
        public int CustomerHeaderId { get; set; }
        public int Status { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
