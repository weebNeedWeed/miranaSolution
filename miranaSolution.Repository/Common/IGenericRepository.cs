namespace miranaSolution.Business.Common
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdEntity(object id);

        Task<bool> InsertAsync(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);
    }
}