using MvcAdvertizer.Data.Models;
using System;

namespace MvcAdvertizer.Services.Interfaces
{
    public interface IAdvertService
    {
        Advert FindById(Guid advertId);

        Advert Create(Advert advert);

        Advert Update(Advert advert);

        void Delete(Advert advert);

        long CountByUserId(Guid userId);
    }
}
