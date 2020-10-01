using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Data.AdditionalObjects;
using MvcAdvertizer.Data.Models;
using System;
using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Interfaces
{
    public interface IAdvertService
    {
        Task<PaginatedList<Advert>> GetFiltredAdverts(AdvertSearchObject searchObject, SortingList sortingObject, RepresentObjectConfigurator pageSizeObject);

        Task<Advert> FindById(Guid advertId);

        Task<Advert> Create(Advert advert);

        Task<Advert> Update(Advert advert);

        Task Delete(Advert advert);

        Task<long> CountByUserId(Guid userId);

        Task DeleteAll();
    }
}
