using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class VmProductSubCategoryAdd
    {
        public int ProductSubCategoryHeaderId { get; set; }
        [Required(ErrorMessage = "Sub-Category Name is required")]
        [Display(Name = "Sub-Category")]
        public string ProductSubCategoryName { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int ProductCategoryHeaderId { get; set; }
    }
}
