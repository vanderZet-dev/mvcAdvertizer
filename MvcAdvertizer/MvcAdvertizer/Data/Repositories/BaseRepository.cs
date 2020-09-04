using MvcAdvertizer.Config.Database;

namespace MvcAdvertizer.Data.Repositories
{
    public class BaseRepository
    {
        protected readonly ApplicationContext source;

        public BaseRepository(ApplicationContext applicationContext) {
            source = applicationContext;
        }
    }
}
