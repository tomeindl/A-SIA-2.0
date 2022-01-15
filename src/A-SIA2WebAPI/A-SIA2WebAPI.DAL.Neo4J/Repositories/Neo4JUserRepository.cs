using A_SIA2WebAPI.DAL.Common;
using A_SIA2WebAPI.Models;
using System.Collections.Generic;

namespace A_SIA2WebAPI.DAL.Neo4J.Repositories
{
    public class Neo4JUserRepository : IRepository<User>
    {
        private readonly Neo4JEngine _engine;

        public Neo4JUserRepository(Neo4JEngine engine)
        {
            _engine = engine;
        }

        public void Delete(long id)
        {
            _engine.Delete<User>(id);
        }

        public User? Get(long id)
        {
            return _engine.Get<User>(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _engine.GetAll<User>();
        }

        public long Insert(User entity)
        {
            var inserted = _engine.Create(entity);
            return inserted != null ? inserted.Id : -1;
        }

        public void Update(User entity)
        {
            _engine.Set(entity);
        }
    }
}
