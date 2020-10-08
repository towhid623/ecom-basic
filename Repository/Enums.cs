using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public enum DbChangeType
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }

    public enum BannerType
    {
        HomeSlider=1
    }

    public enum UserType
    {
        Customer=1,
        Admin=2
    }

    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2,
        Delivered = 3,
        Completed = 4,
    }
}
