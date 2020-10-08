using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class VmOrderAdd
    {
        public VmCustomerAdd Customer { get; set; }
        public IEnumerable<VmPurchase> Purchases { get; set; }
    }
}
