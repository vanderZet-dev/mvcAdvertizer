using System.Collections.Generic;
using System.Linq;

namespace MvcAdvertizer.Config.Tools
{
    public class SortingList<T>
    {
        public List<SortingElement> SortingElements { get; private set; } = new List<SortingElement>();
        public string DefaultSortingElementName { get; private set; }
        public string DefaultSortingDirection { get; private set; }

        public SortingList(string defaultSortingElementName, string defaultSortingDirection)
        {            
            DefaultSortingElementName = defaultSortingElementName;
            DefaultSortingDirection = defaultSortingDirection;
        }

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

        public void ActivateSortingElement(string sortOrderName, string sortDirection, bool notChangeSort)
        {
            if (sortOrderName == null && DefaultSortingDirection != null && DefaultSortingElementName != null) {
                sortOrderName = DefaultSortingElementName;
                if (DefaultSortingDirection.Equals("desc")) sortDirection = "asc";
                if (DefaultSortingDirection.Equals("asc")) sortDirection = "desc";
            }
            
            foreach (var element in SortingElements)
            {
                if (element.SortParam.Equals(sortOrderName)) element.Activate(sortDirection, notChangeSort);
                else element.Deactivate();
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
