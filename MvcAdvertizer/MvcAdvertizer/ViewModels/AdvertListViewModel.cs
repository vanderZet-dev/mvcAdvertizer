using Microsoft.AspNetCore.Mvc.Rendering;
using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Data.AdditionalObjects;
using MvcAdvertizer.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcAdvertizer.ViewModels
{
    public class AdvertListViewModel
    {
        public AdvertSearchObject AdvertSearchObject { get; set; }

        public SortingList SortingObject { get; set; }

        public PaginatedList<AdvertDto> Adverts { get; set; }

        public SelectList UserSearchList { get; set; }

        public Toaster Toaster { get; set; }

        public AdvertListViewModel(AdvertSearchObject advertSearchObject,
                                   SortingList sortingListTool,
                                   Toaster toaster) {
            AdvertSearchObject = advertSearchObject;
            SortingObject = sortingListTool;
            Toaster = toaster;

            InitialSortingListTool();
        }

        private void InitialSortingListTool() {
            SortingObject.AddSortElement(new SortingElement("Number", true));
            SortingObject.AddSortElement(new SortingElement("CreatedOn"));
            SortingObject.AddSortElement(new SortingElement("Content"));
            SortingObject.AddSortElement(new SortingElement("Rate"));
            SortingObject.AddSortElement(new SortingElement("User"));

            SortingObject.ActivateSortingElement();
        }

        public void GenerateUserSearchList(List<UserDto> users) {
            users.Insert(0, new UserDto { Name = "Все", Id = Guid.Empty });
            var selectedElement = users.FirstOrDefault(x => x.Id == AdvertSearchObject.UserId);
            UserSearchList = new SelectList(users, "Id", "Name", selectedElement);
        }
    }
}
