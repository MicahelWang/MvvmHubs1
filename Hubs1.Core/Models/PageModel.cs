using System.Collections.Generic;
using Hubs1.Core.ViewModels;

namespace Hubs1.Core.Models
{
    public class PageModel : BaseViewModel
    {
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPre { get; set; }
        public int Limit { get; set; }
        public List<OrderModel> List { get; set; }
        public string Message { get; set; }
        public int Pagecount { get; set; }
        public int Start { get; set; }
        public bool Subpage { get; set; }
        public bool Success { get; set; }
        public int TotalSize { get; set; }
    }
}
