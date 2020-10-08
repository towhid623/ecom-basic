using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class VmBrandAdd
    {
        public int BrandHeaderId { get; set; }
        [Required(ErrorMessage = "Brand Name is required")]
        public string BrandName { get; set; }
    }
}
