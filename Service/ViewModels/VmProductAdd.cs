using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Service.ViewModels
{
    public class VmProductAdd
    {
        public int ProductHeaderId { get; set; }
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        [Required(ErrorMessage = "Product Sub-Category is required")]
        public int ProductSubCategoryHeaderId { get; set; }
        [Required(ErrorMessage = "Product Category is required")]
        public int ProductCategoryHeaderId { get; set; }
        [Required(ErrorMessage = "Unit is required")]
        public int UnitId { get; set; }
        [Required(ErrorMessage = "Brand Name is required")]
        public int BrandId { get; set; }
        public string ModelNo { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Rate { get; set; }
        public HttpPostedFileBase Image1 { get; set; }
        public HttpPostedFileBase Image2 { get; set; }
        public HttpPostedFileBase Image3 { get; set; }
    }
}
