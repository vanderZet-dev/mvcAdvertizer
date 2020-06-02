
using System.ComponentModel.DataAnnotations;

namespace MvcAdvertizer.Config.Tools
{
    public class SortingElement
    {
        
        public string SortParam { get; private set; }
        public string SortDirection { get; private set; } = "desc";
        public string MirrorSortDirection
        {
            get
            {
                if (SortDirection != null && SortDirection.Equals("desc")) return "asc";
                else return "desc";
            }
        }


        public bool IsActive { get; private set; } = false;
        public bool IsDefault { get; private set; }

        public SortingElement(string sortParam, bool isDefault = false)
        {
            SortParam = sortParam;
            IsDefault = isDefault;
        }

        public void Activate (string sortDirection)
        {
            IsActive = true;            
            SortDirection = sortDirection;
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
