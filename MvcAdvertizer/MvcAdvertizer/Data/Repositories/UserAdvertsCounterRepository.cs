using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.Repositories
{
    public class UserAdvertsCounterRepository : BaseRepository, IUserAdvertsCounter
    {

        public UserAdvertsCounterRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public async Task<UserAdvertsCounter> Add(UserAdvertsCounter obj) {

            source.UsersAdvertsCounters.Add(obj);
            await source.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(UserAdvertsCounter obj) {

            source.UsersAdvertsCounters.Remove(obj);
            await source.SaveChangesAsync();
        }

        public IQueryable<UserAdvertsCounter> FindAll() {

            return source.UsersAdvertsCounters;
        }

        public async Task<UserAdvertsCounter> FindById(Guid guid) {

            return await source.UsersAdvertsCounters.FindAsync(guid);
        }

        public async Task<UserAdvertsCounter> FindByUserId(Guid userId) {

            return await source.UsersAdvertsCounters.Where(x => x.UserId.Equals(userId)).FirstOrDefaultAsync();
        }

        public async Task<long> IncrementCountForUserId(Guid userId) {

            var counter = await FindByUserId(userId);

            if (counter != null)
            {
                bool saveFailed;

                do
                {
                    saveFailed = false;

                    counter.Count++;

                    try
                    {
                        await source.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        saveFailed = true;
                        await e.Entries.Single().ReloadAsync();
                    }
                } while (saveFailed);
            }

            return counter.Count;
        }

        public async Task ResetAllCounters() {

            source.UsersAdvertsCounters.AsQueryable().ForAll(x => x.Count = 0);
            await source.SaveChangesAsync();
        }

        public async Task<UserAdvertsCounter> Update(UserAdvertsCounter obj) {

            source.UsersAdvertsCounters.Update(obj);
            await source.SaveChangesAsync();
            return obj;
        }
    }
}
