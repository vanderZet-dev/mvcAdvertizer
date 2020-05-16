using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Core.ViewModels
{
    public class AdvertListViewModel
    {
        public Guid? SearchedUserId { get; set; }
        public IEnumerable<SelectListItem> UserSearchList { get; set; }        
        public string StringQuerySearch { get; set; }

        public int? selectedPageNumber = 1;
        public int? SelectedPageNumber { get=> selectedPageNumber; set { if (value != null) selectedPageNumber = value; } }
        public int? selectedPageSize;
        public int? SelectedPageSize { get => selectedPageSize; set { if (value != null) selectedPageSize = value; } }

        public SortingList<Advert> SortingListTool { get; set; }

        public PaginatedList<Advert> Adverts { get; set; } 

        public AdvertListViewModel(string defaultSortingElementName, string defaultSortingDirection, int defaultPageSize)
        {
            SortingListTool = new SortingList<Advert>(defaultSortingElementName, defaultSortingDirection);
            SortingListTool.AddSortElement(new SortingElement("Number"));            
            SortingListTool.AddSortElement(new SortingElement("CreatedOn"));            
            SortingListTool.AddSortElement(new SortingElement("Content"));
            SortingListTool.AddSortElement(new SortingElement("Rate"));
            SortingListTool.AddSortElement(new SortingElement("User"));

            SelectedPageSize = defaultPageSize;           
        }          

        public async Task<bool> CreateAsync(IQueryable<Advert> advertSource)
        {   
            advertSource = SortingListTool.ApplySorting(advertSource);
            advertSource = ApplyUserFiltration(advertSource);
            advertSource = ApplyStringQuerySearch(advertSource);

            Adverts = await PaginatedList<Advert>.CreateAsync(advertSource.AsNoTracking(), SelectedPageNumber ?? 1, SelectedPageSize ?? 3);
            return true;
        }
       

        public IEnumerable<SelectListItem> GetUserSearchList(IQueryable<User> userSource)
        {
            // формирования списка пользователей для осущесвтляения поиска по юзерам
             var users = userSource.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            users.Insert(0, new User { Name = "Все", Id = Guid.Empty });
            var selectedElement = users.FirstOrDefault(x => x.Id == SearchedUserId);
            var UserSearchList = new SelectList(users, "Id", "Name", selectedElement);

            return UserSearchList;
        }


        private IQueryable<Advert> ApplyUserFiltration(IQueryable<Advert> advertSource)
        {
            if (SearchedUserId != null && SearchedUserId != Guid.Empty)
            {
                advertSource = advertSource.Where(p => p.UserId == SearchedUserId);
            }

            return advertSource;
        }

        private IQueryable<Advert> ApplyStringQuerySearch(IQueryable<Advert> advertSource)
        {
            if (!string.IsNullOrEmpty(StringQuerySearch))
            {
                advertSource = advertSource.Where(s => s.Number.ToString().Equals(StringQuerySearch)
                                       || (SearchedUserId != null && EF.Functions.Like(s.User.Name.ToUpper(), $"%{StringQuerySearch.ToUpper()}%"))
                                       || EF.Functions.Like(s.Content.ToUpper(), $"%{StringQuerySearch.ToUpper()}%")
                                       );
            }
            return advertSource;
        }
    }
}
