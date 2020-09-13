namespace MvcAdvertizer.Data.Interfaces
{
    public interface IEntityWithUser<Identifier>
    {
        public long CountByUserId(Identifier userId);
    }
}
