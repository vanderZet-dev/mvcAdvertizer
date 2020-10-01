using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MvcAdvertizer.Config;
using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Core.Exceptions;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.Repositories
{
    public class AdvertRepository : BaseRepository, IAdverts
    {
        private readonly long advertsLimit;
        private readonly IUserAdvertsCounter userAdvertsCounterRepository;
        private readonly ILogger<AdvertRepository> logger;

        public AdvertRepository(ApplicationContext applicationContext,
                                IUserAdvertsCounter userAdvertsCounterRepository,
                                IOptions<UsersAdvertsSettings> usersAdvertsSettings,
                                ILogger<AdvertRepository> logger)
            : base(applicationContext) {
            this.userAdvertsCounterRepository = userAdvertsCounterRepository;
            advertsLimit = usersAdvertsSettings.Value.MaxUserAdvertsCount;
            this.logger = logger;
        }

        public IQueryable<Advert> FindAll() {

            return source.Adverts.Include(x => x.User);
        }

        public async Task<Advert> FindById(Guid guid) {

            return await source.Adverts.Where(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public async Task<Advert> Add(Advert obj) {

            using (var transaction = source.Database.BeginTransaction())
            {
                try
                {
                    source.Adverts.Add(obj);
                    await source.SaveChangesAsync();
                    var currentCount = await userAdvertsCounterRepository.IncrementCountForUserId(obj.UserId);

                    if (advertsLimit < currentCount)
                    {
                        throw new UserAdvertLimitExceededException();
                    }

                    await transaction.CommitAsync();
                }
                catch (UserAdvertLimitExceededException ex)
                {
                    await transaction.RollbackAsync();
                    logger.LogError(ex.Message);
                    throw new UserAdvertLimitExceededException();
                }
            }

            return obj;
        }

        public async Task<Advert> Update(Advert obj) {

            source.Adverts.Update(obj);
            source.Entry(obj).Property(x => x.ImageHash).IsModified = false;
            source.Entry(obj).Property(x => x.CreatedOn).IsModified = false;
            await source.SaveChangesAsync();

            return obj;
        }

        public async Task Delete(Advert obj) {

            source.Adverts.Remove(obj);
            await source.SaveChangesAsync();
        }

        public long CountByUserId(Guid userId) {

            var count = source.Adverts.Where(x => x.UserId == userId).Count();
            return count;
        }

        public async Task DeleteAll() {

            source.RemoveRange(source.Adverts);
            await source.SaveChangesAsync();

            await userAdvertsCounterRepository.ResetAllCounters();
        }
    }
}
