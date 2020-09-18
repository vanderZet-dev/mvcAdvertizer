using MvcAdvertizer.Data.Models;
using System;

namespace MvcAdvertizer.Data.Interfaces
{
    public interface IAdverts : IRepository<Advert, Guid>, IEntityWithUser<Guid>
    {

    }
}
