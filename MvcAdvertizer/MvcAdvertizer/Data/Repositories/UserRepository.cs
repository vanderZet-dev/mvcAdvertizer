using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using System;
using System.Linq;

namespace MvcAdvertizer.Data.Repositories
{
    public class UserRepository : BaseRepository, IUsers
    {       

        public UserRepository(ApplicationContext applicationContext):base(applicationContext) {}

        public IQueryable<User> findAll() {

             return source.Users;
        }

        public IQueryable<User> findById(Guid guid) {

            return source.Users.Where(x=>x.Id.Equals(guid));
        }

        public User Save(User obj) {

            source.Users.Add(obj);
            source.SaveChanges();

            return obj;
        }
    }
}
