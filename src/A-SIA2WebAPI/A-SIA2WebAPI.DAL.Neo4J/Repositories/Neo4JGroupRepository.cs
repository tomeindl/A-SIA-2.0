using A_SIA2WebAPI.DAL.Common;
using A_SIA2WebAPI.Models;
using System.Collections.Generic;

namespace A_SIA2WebAPI.DAL.Neo4J.Repositories
{
    public class Neo4JGroupRepository : IRepository<Group>
    {
        private readonly Neo4JEngine _engine;

        public Neo4JGroupRepository(Neo4JEngine engine)
        {
            _engine = engine;
        }

        public void Delete(long id)
        {
            _engine.Delete<Group>(id);
        }

        public Group? Get(long id)
        {
            return _engine.Get<Group>(id);
        }

        public IEnumerable<Group> GetAll()
        {
            return _engine.GetAll<Group>();
        }

        public long Insert(Group entity)
        {
            var result = _engine.Create(entity);
            return result != null ? result.Id : -1;
        }

        public void Update(Group entity)
        {
            _engine.Set(entity);
        }
    }
}
