using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class FndFlexValue:BaseEntity
    {
        [Key]
        public int FlexValueId { get; set; }
        [Required]
        [Display(Name = "Config Value")]
        public string FlexValue { get; set; }
        [Display(Name = "Config Short Value")]
        public string FlexShortValue { get; set; }
        public int FlexValueSetId { get; set; }
        public int? ParentFlexValueId { get; set; }
        public int? ParentFlexValueSetId { get; set; }
        public bool? Status { get; set; }
        public virtual FndFlexValueSet FndFlexValueSet { get; set; }
    }
}
