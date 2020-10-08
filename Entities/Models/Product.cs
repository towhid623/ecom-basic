using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Product : BaseEntity
    {
        [Key]
        public int ProductHeaderId { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public int ProductSubCategoryHeaderId { get; set; }
        public int UnitId { get; set; }
        public int BrandId { get; set; }
        public string ModelNo { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public virtual ProductSubCategory ProductSubCategory { get; set; }
    }
}
