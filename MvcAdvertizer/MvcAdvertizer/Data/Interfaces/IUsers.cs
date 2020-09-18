using MvcAdvertizer.Data.Models;
using System;

namespace MvcAdvertizer.Data.Interfaces
{
    public interface IUsers : IRepository<User, Guid>
    {
        
    }
}
