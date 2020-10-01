using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.Repositories
{
    public class UserRepository : BaseRepository, IUsers
    {        
        public UserRepository(ApplicationContext applicationContext) 
            : base(applicationContext) {            
        }

        public IQueryable<User> FindAll() {

             return source.Users;
        }

        public async Task<User> FindById(Guid guid) {

            return await source.Users.Where(x=>x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public async Task<User> Add(User obj) {

            source.Users.Add(obj);
            source.UsersAdvertsCounters.Add(new UserAdvertsCounter() { UserId = obj.Id, Count = 0 });
            await source.SaveChangesAsync();

            return obj;
        }

        public async Task<User> Update(User obj) {

            source.Users.Update(obj);
            await source.SaveChangesAsync();

            return obj;
        }

        public async Task Delete(User obj) {

            source.Users.Remove(obj);
            await source.SaveChangesAsync();
        }
    }
}
