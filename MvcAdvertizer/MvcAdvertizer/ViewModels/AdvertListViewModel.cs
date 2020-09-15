using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Data.AdditionalObjects;
using MvcAdvertizer.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.ViewModels
{
    public class AdvertListViewModel
    {
        public RepresentObjectConfigurator RepresentObjectConfigurator { get; set; }

        public AdvertSearchObject AdvertSearchObject { get; set; }

        public SortingList<Advert> SortingListTool { get; set; }


        public PaginatedList<Advert> Adverts { get; set; }

        public SelectList UserSearchList { get; set; }

        public Toaster Toaster { get; set; }

        public AdvertListViewModel(RepresentObjectConfigurator representObjectConfigurator, 
                                    AdvertSearchObject advertSearchObject,
                                    SortingList<Advert> sortingListTool,
                                    Toaster toaster)
        {
            AdvertSearchObject = advertSearchObject;
            RepresentObjectConfigurator = representObjectConfigurator;
            SortingListTool = sortingListTool;
            Toaster = toaster;

            InitialSortingListTool();            
        }       

        private void InitialSortingListTool()
        {
            SortingListTool.AddSortElement(new SortingElement("Number", true));
            SortingListTool.AddSortElement(new SortingElement("CreatedOn"));
            SortingListTool.AddSortElement(new SortingElement("Content"));
            SortingListTool.AddSortElement(new SortingElement("Rate"));
            SortingListTool.AddSortElement(new SortingElement("User"));

            SortingListTool.ActivateSortingElement();
        }

        public async Task<bool> CreateAsync(IQueryable<Advert> advertSource)
        {
            advertSource = ApplyDeletedSearch(advertSource);
            advertSource = SortingListTool.ApplySorting(advertSource);
            advertSource = ApplyUserFiltration(advertSource);
            advertSource = ApplyStringQuerySearch(advertSource);
            advertSource = ApplyDateSearch(advertSource);            

            Adverts = await PaginatedList<Advert>.CreateAsync(advertSource.AsNoTracking(), RepresentObjectConfigurator.PageNumber ?? 1, RepresentObjectConfigurator.PageSize ?? 3);
            return true;
        }       

        public void GenerateUserSearchList(IQueryable<User> userSource)
        {
            // формирования списка пользователей для осущесвтляения поиска по юзерам
             var users = userSource.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            users.Insert(0, new User { Name = "Все", Id = Guid.Empty });
            var selectedElement = users.FirstOrDefault(x => x.Id == AdvertSearchObject.UserId);
            UserSearchList = new SelectList(users, "Id", "Name", selectedElement);            
        }

        private IQueryable<Advert> ApplyUserFiltration(IQueryable<Advert> advertSource)
        {
            if (AdvertSearchObject.UserId != null && AdvertSearchObject.UserId != Guid.Empty)
            {
                advertSource = advertSource.Where(p => p.UserId == AdvertSearchObject.UserId);
            }

            return advertSource;
        }

        private IQueryable<Advert> ApplyStringQuerySearch(IQueryable<Advert> advertSource)
        {
            if (!string.IsNullOrEmpty(AdvertSearchObject.SearchStringQuery))
            {
                advertSource = advertSource.Where(s => s.Number.ToString().Equals(AdvertSearchObject.SearchStringQuery)
                                       || (AdvertSearchObject.UserId != null && EF.Functions.Like(s.User.Name.ToUpper(), $"%{AdvertSearchObject.SearchStringQuery.ToUpper()}%"))
                                       || EF.Functions.Like(s.Content.ToUpper(), $"%{AdvertSearchObject.SearchStringQuery.ToUpper()}%")
                                       || s.Rate.ToString().Equals(AdvertSearchObject.SearchStringQuery)
                                       );
            }
            return advertSource;
        }

        private IQueryable<Advert> ApplyDateSearch(IQueryable<Advert> advertSource)
        {
            if (AdvertSearchObject.DateStartSearch != null)
            {
                advertSource = advertSource.Where(s => s.CreatedOn >= AdvertSearchObject.DateStartSearch);
            }

            if (AdvertSearchObject.DateEndSearch != null)
            {
                advertSource = advertSource.Where(s => s.CreatedOn <= AdvertSearchObject.DateEndSearch);
            }

            return advertSource;
        }

        private IQueryable<Advert> ApplyDeletedSearch(IQueryable<Advert> advertSource) {
            
             advertSource = advertSource.Where(s => s.Deleted == false);           

            return advertSource;
        }
    }
}
