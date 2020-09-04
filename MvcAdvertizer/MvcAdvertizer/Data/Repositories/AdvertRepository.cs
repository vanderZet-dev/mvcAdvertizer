using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.Repositories
{
    public class AdvertRepository : BaseRepository, IAdverts
    {

        public AdvertRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public IQueryable<Advert> findAll() {

            return source.Adverts;
        }

        public IQueryable<Advert> findById(Guid guid) {

            return source.Adverts.Where(x => x.Id.Equals(guid));
        }

        public Advert Save(Advert obj) {

            source.Adverts.Add(obj);
            source.SaveChanges();

            return obj;
        }
    }
}
