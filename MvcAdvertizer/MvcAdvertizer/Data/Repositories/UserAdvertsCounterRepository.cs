using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using System;
using System.Linq;

namespace MvcAdvertizer.Data.Repositories
{
    public class UserAdvertsCounterRepository : BaseRepository, IUserAdvertsCounter
    {

        public UserAdvertsCounterRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public UserAdvertsCounter Add(UserAdvertsCounter obj) {

            source.UsersAdvertsCounters.Add(obj);
            source.SaveChanges();
            return obj;
        }

        public void Delete(UserAdvertsCounter obj) {

            source.UsersAdvertsCounters.Remove(obj);
            source.SaveChanges();
        }

        public IQueryable<UserAdvertsCounter> FindAll() {

            return source.UsersAdvertsCounters;
        }

        public UserAdvertsCounter FindById(Guid guid) {

            return source.UsersAdvertsCounters.Find(guid);
        }

        public UserAdvertsCounter FindByUserId(Guid userId) {

            return source.UsersAdvertsCounters.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
        }

        public long IncrementCountForUserId(Guid userId) {

            var counter = FindByUserId(userId);

            if (counter != null)
            {
                bool saveFailed;

                do
                {
                    saveFailed = false;

                    counter.Count++;

                    try
                    {
                        source.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        saveFailed = true;
                        e.Entries.Single().Reload();
                    }
                } while (saveFailed);
            }

            return counter.Count;
        }

        public void ResetAllCounters() {

            source.UsersAdvertsCounters.AsQueryable().ForAll(x => x.Count = 0);
            source.SaveChanges();
        }

        public UserAdvertsCounter Update(UserAdvertsCounter obj) {

            source.UsersAdvertsCounters.Update(obj);
            source.SaveChanges();
            return obj;
        }
    }
}
