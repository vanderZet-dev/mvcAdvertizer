using System.Linq;

namespace MvcAdvertizer.Data.Interfaces
{
    public interface IRepository<Type, Identifier>
    {
        public IQueryable<Type> findAll();

        public IQueryable<Type> findById(Identifier guid);

        public Type Save(Type obj);
    }
}
