using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Data.AdditionalObjects;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using MvcAdvertizer.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Implementations
{
    public class AdvertService : IAdvertService
    {
        private readonly IAdverts advertsRepository;
        private readonly IUserAdvertsCounter userAdvertsCounterRepository;

        public AdvertService(IAdverts advertsRepository,
                             IUserAdvertsCounter userAdvertsCounterRepository) {
            this.advertsRepository = advertsRepository;
            this.userAdvertsCounterRepository = userAdvertsCounterRepository;
        }

        public async Task<Advert> Create(Advert advert) {

            return await advertsRepository.Add(advert);
        }

        public async Task Delete(Advert advert) {

            await advertsRepository.Delete(advert);
        }

        public async Task<Advert> FindById(Guid advertId) {

            return await advertsRepository.FindById(advertId);
        }

        public async Task<Advert> Update(Advert advert) {

            return await advertsRepository.Update(advert);
        }

        public async Task<long> CountByUserId(Guid userId) {

            long currentCount = 0;

            var counter = await userAdvertsCounterRepository.FindByUserId(userId);
            if (counter != null)
            {
                currentCount = counter.Count;
            }

            return currentCount;
        }

        public async Task<PaginatedList<Advert>> GetFiltredAdverts(AdvertSearchObject searchObject, SortingList sortingObject) {

            var advertsSouce = advertsRepository.FindAll();
            advertsSouce = ApplyDeletedSearch(advertsSouce);
            advertsSouce = ApplyStringQuerySearch(advertsSouce, searchObject);
            advertsSouce = ApplyUserFiltration(advertsSouce, searchObject);
            advertsSouce = ApplyDateSearch(advertsSouce, searchObject);
            advertsSouce = ApplySorting(advertsSouce, sortingObject);

            return await PaginatedList<Advert>.CreateAsync(advertsSouce.AsNoTracking(), searchObject.PageNumber ?? 1, searchObject.PageSize ?? 3);
        }

        private IQueryable<Advert> ApplyDeletedSearch(IQueryable<Advert> advertSource) {

            advertSource = advertSource.Where(s => s.Deleted == false);

            return advertSource;
        }

        private IQueryable<Advert> ApplyStringQuerySearch(IQueryable<Advert> advertSource, AdvertSearchObject searchObject) {
            if (!string.IsNullOrEmpty(searchObject.SearchStringQuery))
            {
                advertSource = advertSource.Where(s => s.Number.ToString().Equals(searchObject.SearchStringQuery)
                                       || (searchObject.UserId != null && EF.Functions.Like(s.User.Name.ToUpper(), $"%{searchObject.SearchStringQuery.ToUpper()}%"))
                                       || EF.Functions.Like(s.Content.ToUpper(), $"%{searchObject.SearchStringQuery.ToUpper()}%")
                                       || s.Rate.ToString().Equals(searchObject.SearchStringQuery)
                                       );
            }
            return advertSource;
        }

        private IQueryable<Advert> ApplyUserFiltration(IQueryable<Advert> advertSource, AdvertSearchObject searchObject) {
            if (searchObject.UserId != null && searchObject.UserId != Guid.Empty)
            {
                advertSource = advertSource.Where(p => p.UserId == searchObject.UserId);
            }

            return advertSource;
        }

        private IQueryable<Advert> ApplyDateSearch(IQueryable<Advert> advertSource, AdvertSearchObject searchObject) {
            if (searchObject.DateStartSearch != null)
            {
                advertSource = advertSource.Where(s => s.CreatedOn >= searchObject.DateStartSearch);
            }

            if (searchObject.DateEndSearch != null)
            {
                advertSource = advertSource.Where(s => s.CreatedOn <= searchObject.DateEndSearch);
            }

            return advertSource;
        }

        private IQueryable<Advert> ApplySorting(IQueryable<Advert> advertSource, SortingList sortingObject) {
            var activeSortElement = sortingObject.GetActiveSortingElement();
            if (activeSortElement != null)
            {

                switch (activeSortElement.SortDirection)
                {
                    case "asc":
                        advertSource = DynamicQuableSortingGenerator.OrderBy(advertSource, activeSortElement.SortParam);
                        break;
                    case "desc":
                        advertSource = DynamicQuableSortingGenerator.OrderByDescending(advertSource, activeSortElement.SortParam);
                        break;
                }
            }
            return advertSource;
        }

        public async Task DeleteAll() {

            await advertsRepository.DeleteAll();
        }
    }
}
