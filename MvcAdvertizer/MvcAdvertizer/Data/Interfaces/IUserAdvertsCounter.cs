using MvcAdvertizer.Data.Models;
using System;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.Interfaces
{
    public interface IUserAdvertsCounter : IRepository<UserAdvertsCounter, Guid>
    {
        Task<long> IncrementCountForUserId(Guid userId);

        Task<UserAdvertsCounter> FindByUserId(Guid userId);

        Task ResetAllCounters();
    }
}
