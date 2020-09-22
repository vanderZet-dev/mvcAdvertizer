using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using MvcAdvertizer.Services.Interfaces;
using System;

namespace MvcAdvertizer.Services.Implementations
{
    public class AdvertService : IAdvertService
    {
        private readonly IAdverts advertsRepository;

        public AdvertService(IAdverts advertsRepository) {
            this.advertsRepository = advertsRepository;
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

            return advertsRepository.CountByUserId(userId);            
        }
    }
}
