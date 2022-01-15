using System.Collections.Generic;

namespace A_SIA2WebAPI.DAL.Common
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T? Get(long id);
        long Insert(T entity);
        void Update(T entity);
        void Delete(long id);
    }
}
