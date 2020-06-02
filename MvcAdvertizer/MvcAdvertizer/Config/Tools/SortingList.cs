using System.Collections.Generic;
using System.Linq;

namespace MvcAdvertizer.Config.Tools
{
    public class SortingList<T>
    {        
        public string SortOrderName { get; set; }        
        public string SortDirection { get; set; }    
        
        public string MirrorSortDirection
        {
            get
            {
                if (SortDirection != null && SortDirection.Equals("desc")) return "asc";
                else return "desc";
            }
        }        

        public List<SortingElement> SortingElements { get; private set; } = new List<SortingElement>();

        public void AddSortElement(SortingElement sortingElement)
        {
            if (!SortingElements.Exists(x => x.SortParam.Equals(sortingElement.SortParam))){
                SortingElements.Add(sortingElement);
            }
        }

        public SortingElement GetSortingElementByName(string elementsName)
        {
            return SortingElements.FirstOrDefault(x => x.SortParam.Equals(elementsName));
        }

        public void ActivateSortingElement()
        {
            SetDefaultSorting();
            foreach (var element in SortingElements)
            {
                if (element.SortParam.Equals(SortOrderName)) element.Activate(SortDirection);
                else element.Deactivate();
            }
        }

        private void SetDefaultSorting()
        {
            if (SortOrderName == null)
            {
                var defaultSortingElementName = SortingElements.FirstOrDefault(x => x.IsDefault);
                if (defaultSortingElementName != null)
                {
                    SortOrderName = defaultSortingElementName.SortParam;
                    SortDirection = defaultSortingElementName.SortDirection;
                }
            }
        }

        public SortingElement GetActiveSortingElement()
        {
            return SortingElements.FirstOrDefault(x => x.IsActive);
        }

        public IQueryable<T> ApplySorting (IQueryable<T> source)
        {
            var activeSortElement = GetActiveSortingElement();
            if (activeSortElement != null)
            {

                switch (activeSortElement.SortDirection)
                {
                    case "asc":
                        source = DynamicQuableSortingGenerator.OrderBy(source, activeSortElement.SortParam);
                        break;
                    case "desc":
                        source = DynamicQuableSortingGenerator.OrderByDescending(source, activeSortElement.SortParam);
                        break;
                }
            }
            return source;
        }

        
    }
}
