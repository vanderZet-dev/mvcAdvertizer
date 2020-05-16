using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Config.Tools
{
    public class SortingElement
    {
        public string SortParam { get; private set; }
        public string SortDirection { get; private set; } = "desc";
        public bool IsActive { get; private set; } = false;        

        public SortingElement(string sortParam)
        {
            SortParam = sortParam;
        }

        public void Activate (string sortDirection, bool notChangeSort)
        {
            IsActive = true;
            if (notChangeSort)
            {
                SortDirection = sortDirection;
            }
            else SortDirection = sortDirection == "asc" ? "desc" : "asc";
        }

        public void Deactivate ()
        {
            IsActive = false;
            SortDirection = "desc";
        }

        public override string ToString()
        {
            return SortParam;
        }
    }
}
