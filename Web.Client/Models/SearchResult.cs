using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Client.Models
{
    public class SearchResult
    {
        public IPagedList<Employee> Employees { get; set; }

        public string SearchQuery { get; set; }
    }
}
