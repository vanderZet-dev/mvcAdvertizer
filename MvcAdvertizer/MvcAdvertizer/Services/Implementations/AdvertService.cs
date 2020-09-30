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

        public Advert Create(Advert advert) {

            return advertsRepository.Add(advert);
        }

        public void Delete(Advert advert) {

            advertsRepository.Delete(advert);
        }

        public Advert FindById(Guid advertId) {

            return advertsRepository.FindById(advertId);
        }

        public Advert Update(Advert advert) {

            return advertsRepository.Update(advert);
        }

        public long CountByUserId(Guid userId) {

            long currentCount = 0;

            var counter = userAdvertsCounterRepository.FindByUserId(userId);
            if (counter != null)
            {
                currentCount = counter.Count;
            }

            return currentCount;
        }

        public async Task<PaginatedList<Advert>> GetFiltredAdverts(AdvertSearchObject searchObject, SortingList sortingObject, RepresentObjectConfigurator pageSizeObject) {

            var advertsSouce = advertsRepository.FindAll();
            advertsSouce = ApplyDeletedSearch(advertsSouce);
            advertsSouce = ApplyStringQuerySearch(advertsSouce, searchObject);
            advertsSouce = ApplyUserFiltration(advertsSouce, searchObject);
            advertsSouce = ApplyDateSearch(advertsSouce, searchObject);
            advertsSouce = ApplySorting(advertsSouce, sortingObject);

            return await PaginatedList<Advert>.CreateAsync(advertsSouce.AsNoTracking(), pageSizeObject.PageNumber ?? 1, pageSizeObject.PageSize ?? 3);
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

        public void DeleteAll() {

            advertsRepository.DeleteAll();
        }
    }
}
