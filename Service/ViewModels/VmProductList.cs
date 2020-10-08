using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class VmProductList
    {
        public int ProductHeaderId { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public string BrandName { get; set; }
        public string UnitName { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
    }
}
