using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.Interfaces
{
    public interface IRepository<Type, Identifier>
    {
        public IQueryable<Type> FindAll();

        public Task<Type> FindById(Identifier guid);

        public Task<Type> Add(Type obj);

        public Task<Type> Update(Type obj);

        public Task Delete(Type obj);
    }
}
