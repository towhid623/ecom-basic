using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service.ViewModels
{
    public class VmBanner
    {
        public int BannerHeaderId { get; set; }
        public int? BannerTypeId { get; set; }
        public string BannerImgUrl { get; set; }
        public string BannerTitle { get; set; }
        public string BannerDescription { get; set; }
        public HttpPostedFileBase BannerFile { get; set; }
    }
}
