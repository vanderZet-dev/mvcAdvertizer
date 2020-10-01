using MvcAdvertizer.Data.Models;
using System;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.Interfaces
{
    public interface IAdverts : IRepository<Advert, Guid>
    {
        Task DeleteAll();
    }
}
