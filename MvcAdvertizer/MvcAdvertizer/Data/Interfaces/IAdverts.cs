using MvcAdvertizer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.Interfaces
{
    public interface IAdverts : IRepository<Advert, Guid>
    {

    }
}
