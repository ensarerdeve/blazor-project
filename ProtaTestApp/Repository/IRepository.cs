using MongoDB.Bson;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(ObjectId id);
    Task Insert(T entity);
    Task Update(ObjectId id, T entity);
    Task Delete(ObjectId id);
}
