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

        public IQueryable<User> FindAll() {

             return source.Users;
        }

        public User FindById(Guid guid) {

            return source.Users.Where(x=>x.Id.Equals(guid)).FirstOrDefault();
        }

        public User Add(User obj) {

            source.Users.Add(obj);
            source.SaveChanges();

            return obj;
        }

        public User Update(User obj) {

            source.Users.Update(obj);
            source.SaveChanges();

            return obj;
        }

        public void Delete(User obj) {

            source.Users.Remove(obj);
            source.SaveChanges();
        }
    }
}
