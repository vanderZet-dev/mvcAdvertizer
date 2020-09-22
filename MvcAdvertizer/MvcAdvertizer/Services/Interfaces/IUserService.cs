using MvcAdvertizer.Data.Models;
using System;
using System.Linq;

namespace MvcAdvertizer.Services.Interfaces
{
    public interface IUserService
    {
        IQueryable<User> FindAll();

        User FindById(Guid guid);

        User Create(User obj);

        User Update(User obj);

        void Delete(User obj);
    }
}