using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ProductCategory:BaseEntity
    {
        [Key]
        public int ProductCategoryHeaderId { get; set; }
        [Required(ErrorMessage = "Category Name is required")]
        [Display(Name = "Category")]
        public string ProductCategoryName { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<ProductSubCategory> ProductSubCategories { get; set; }
    }
}
