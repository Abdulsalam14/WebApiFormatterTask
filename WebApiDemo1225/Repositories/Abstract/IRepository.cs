using System.Linq.Expressions;

namespace WebApiDemo1225.Repositories.Abstract
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T,bool>> expression);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
