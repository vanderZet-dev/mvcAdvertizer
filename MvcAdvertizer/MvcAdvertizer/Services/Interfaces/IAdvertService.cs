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

        Advert FindById(Guid advertId);

        Advert Create(Advert advert);

        Advert Update(Advert advert);

        void Delete(Advert advert);

        long CountByUserId(Guid userId);

        void DeleteAll();
    }
}
