using MvcAdvertizer.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> FindAll();

        Task<User> FindById(Guid guid);

        Task<User> Create(User obj);

        Task<User> Update(User obj);

        Task Delete(User obj);
    }
}