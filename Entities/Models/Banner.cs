using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Banner:BaseEntity
    {
        [Key]
        public int BannerHeaderId { get; set; }
        public int? BannerTypeId { get; set; }
        public string BannerImgUrl { get; set; }
        public string BannerTitle { get; set; }
        public string BannerDescription { get; set; }
    }
}
