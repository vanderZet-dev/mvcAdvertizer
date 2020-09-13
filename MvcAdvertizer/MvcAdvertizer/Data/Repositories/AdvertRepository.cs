using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using System;
using System.Linq;

namespace MvcAdvertizer.Data.Repositories
{
    public class AdvertRepository : BaseRepository, IAdverts
    {

        public AdvertRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public IQueryable<Advert> findAll() {

            return source.Adverts;
        }

        public Advert findById(Guid guid) {

            return source.Adverts.Where(x => x.Id.Equals(guid)).FirstOrDefault();
        }

        public Advert Add(Advert obj) {
            
            source.Adverts.Add(obj);
            source.SaveChanges();            

            return obj;
        }

        public Advert Update(Advert obj) {
            
            source.Adverts.Update(obj);
            source.Entry(obj).Property(x => x.Image).IsModified = false;
            source.SaveChanges();

            return obj;
        }

        public void Delete(Advert obj) {

            source.Adverts.Remove(obj);
            source.SaveChanges();            
        }        

        public long CountByUserId(Guid userId) {

            var count = source.Adverts.Where(x => x.UserId == userId).Count();
            return count;
        }
    }
}
