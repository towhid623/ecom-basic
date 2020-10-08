using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class FndFlexValueSet:BaseEntity
    {
        [Key]
        public int FlexValueSetId { get; set; }
        [Required]
        [Display(Name = "Config Value Set Name")]
        public string FlexValueSetName { get; set; }
        [Display(Name = "Config Value Set Short Name")]
        public string FlexValueSetShortName { get; set; }
        public virtual ICollection<FndFlexValue> FndFlexValue { get; set; }
    }
}
