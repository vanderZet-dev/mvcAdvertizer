using System.Linq;

namespace MvcAdvertizer.Data.Interfaces
{
    public interface IRepository<Type, Identifier>
    {
        public IQueryable<Type> FindAll();

        public Type FindById(Identifier guid);

        public Type Add(Type obj);

        public Type Update(Type obj);

        public void Delete(Type obj);
    }
}
