using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDemon.Models
{
    public class CompanySelectListItem
    {
        public CompanySelectListItem(int id, string name)
        {
            CompanyId = id;
            Name = name;
        }
        public int CompanyId { get; set; }
        public string Name { get; set; }
    }
}
