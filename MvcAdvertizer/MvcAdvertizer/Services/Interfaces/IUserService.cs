using MvcAdvertizer.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Interfaces
{
    public interface IUserService
    {
        IQueryable<User> FindAll();

        Task<User> FindById(Guid guid);

        Task<User> Create(User obj);

        Task<User> Update(User obj);

        Task Delete(User obj);
    }
}