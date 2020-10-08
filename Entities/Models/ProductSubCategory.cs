using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ProductSubCategory : BaseEntity
    {
        [Key]
        public int ProductSubCategoryHeaderId { get; set; }
        [Required]
        [Display(Name = "Sub-Category")]
        public string ProductSubCategoryName { get; set; }
        public string ImageUrl { get; set; }
        public int ProductCategoryHeaderId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
