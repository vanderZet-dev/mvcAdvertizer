using MvcAdvertizer.Data.Models;
using System;

namespace MvcAdvertizer.Data.Interfaces
{
    public interface IUserAdvertsCounter : IRepository<UserAdvertsCounter, Guid>
    {
        long IncrementCountForUserId(Guid userId);

        UserAdvertsCounter FindByUserId(Guid userId);

        void ResetAllCounters();
    }
}
