using System.Linq;

namespace MvcAdvertizer.Data.Interfaces
{
    public interface IRepository<Type, Identifier>
    {
        public IQueryable<Type> findAll();

        public Type findById(Identifier guid);

        public Type Save(Type obj);

        public Type Update(Type obj);

        public void Delete(Type obj);
    }
}
