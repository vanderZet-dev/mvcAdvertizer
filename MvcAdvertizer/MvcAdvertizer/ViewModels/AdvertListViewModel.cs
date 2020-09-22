using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Data.AdditionalObjects;
using MvcAdvertizer.Data.DTO;
using MvcAdvertizer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcAdvertizer.ViewModels
{
    public class AdvertListViewModel
    {
        public RepresentObjectConfigurator RepresentObjectConfigurator { get; set; }

        public AdvertSearchObject AdvertSearchObject { get; set; }

        public SortingList SortingObject { get; set; }

        public PaginatedList<AdvertDto> Adverts { get; set; }

        public SelectList UserSearchList { get; set; }

        public Toaster Toaster { get; set; }

        public AdvertListViewModel(RepresentObjectConfigurator representObjectConfigurator, 
                                    AdvertSearchObject advertSearchObject,
                                    SortingList sortingListTool,
                                    Toaster toaster)
        {
            AdvertSearchObject = advertSearchObject;
            RepresentObjectConfigurator = representObjectConfigurator;
            SortingObject = sortingListTool;
            Toaster = toaster;

            InitialSortingListTool();            
        }       

        private void InitialSortingListTool()
        {
            SortingObject.AddSortElement(new SortingElement("Number", true));
            SortingObject.AddSortElement(new SortingElement("CreatedOn"));
            SortingObject.AddSortElement(new SortingElement("Content"));
            SortingObject.AddSortElement(new SortingElement("Rate"));
            SortingObject.AddSortElement(new SortingElement("User"));

            SortingObject.ActivateSortingElement();
        }       

        public void GenerateUserSearchList(List<UserDto> users)
        {   
            users.Insert(0, new UserDto { Name = "Все", Id = Guid.Empty });
            var selectedElement = users.FirstOrDefault(x => x.Id == AdvertSearchObject.UserId);
            UserSearchList = new SelectList(users, "Id", "Name", selectedElement);            
        }        
    }
}
