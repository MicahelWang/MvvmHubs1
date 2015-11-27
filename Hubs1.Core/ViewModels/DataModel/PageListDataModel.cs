using System.Collections.Generic;

namespace Hubs1.Core.ViewModels.DataModel
{
    public class PageListDataModel<T> : BaseViewModel
    {
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPre { get; set; }
        public int Limit { get; set; }
        public List<T> List { get; set; }
        public string Message { get; set; }
        public int Pagecount { get; set; }
        public new int Start { get; set; }
        public bool Subpage { get; set; }
        public bool Success { get; set; }
        public int TotalSize { get; set; }
    }
}
