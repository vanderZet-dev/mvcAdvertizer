using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MvcAdvertizer.Config;
using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Core.Exceptions;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using System;
using System.Linq;

namespace MvcAdvertizer.Data.Repositories
{
    public class AdvertRepository : BaseRepository, IAdverts
    {
        private readonly long advertsLimit;
        private readonly IUserAdvertsCounter userAdvertsCounterRepository;

        public AdvertRepository(ApplicationContext applicationContext,
                                IUserAdvertsCounter userAdvertsCounterRepository,
                                IOptions<AppSettings> settings) : base(applicationContext) {
            this.userAdvertsCounterRepository = userAdvertsCounterRepository;
            advertsLimit = settings.Value.UsersAdvertsSettings.MaxUserAdvertsCount;
        }

        public IQueryable<Advert> FindAll() {

            return source.Adverts.Include(x => x.User);
        }

        public Advert FindById(Guid guid) {

            return source.Adverts.Where(x => x.Id.Equals(guid)).FirstOrDefault();
        }

        public Advert Add(Advert obj) {

            using (var transaction = source.Database.BeginTransaction())
            {
                try
                {
                    source.Adverts.Add(obj);
                    source.SaveChanges();
                    var currentCount = userAdvertsCounterRepository.IncrementCountForUserId(obj.UserId);

                    if (advertsLimit < currentCount)
                    {
                        throw new UserAdvertLimitExceededException();
                    }

                    transaction.Commit();
                }
                catch (UserAdvertLimitExceededException ex)
                {
                    transaction.Rollback();
                    throw new UserAdvertLimitExceededException();
                }
            }

            return obj;
        }

        public Advert Update(Advert obj) {

            source.Adverts.Update(obj);
            source.Entry(obj).Property(x => x.ImageHash).IsModified = false;
            source.Entry(obj).Property(x => x.CreatedOn).IsModified = false;
            source.SaveChanges();

            return obj;
        }

        public void Delete(Advert obj) {

            source.Adverts.Remove(obj);
            source.SaveChanges();
        }

        public long CountByUserId(Guid userId) {

            var count = source.Adverts.Where(x => x.UserId == userId).Count();
            return count;
        }

        public void DeleteAll() {

            source.RemoveRange(source.Adverts);
            source.SaveChanges();

            userAdvertsCounterRepository.ResetAllCounters();
        }
    }
}
