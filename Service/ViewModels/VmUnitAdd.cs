using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class VmUnitAdd
    {
        public int UnitHeaderId { get; set; }
        [Required(ErrorMessage = "Unit Name is required")]
        [Display(Name = "Unit")]
        public string UnitName { get; set; }
    }
}
