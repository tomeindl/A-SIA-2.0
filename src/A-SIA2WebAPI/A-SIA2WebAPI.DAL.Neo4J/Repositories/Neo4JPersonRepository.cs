using A_SIA2WebAPI.DAL.Common;
using A_SIA2WebAPI.Models;
using System.Collections.Generic;

namespace A_SIA2WebAPI.DAL.Neo4J.Repositories
{
    public class Neo4JPersonRepository : IRepository<Person>
    {
        private readonly Neo4JEngine _engine;

        public Neo4JPersonRepository(Neo4JEngine engine)
        {
            _engine = engine;
        }

        public void Delete(long id)
        {
            _engine.Delete<Person>(id);
        }

        public Person? Get(long id)
        {
            return _engine.Get<Person>(id);
        }

        public IEnumerable<Person> GetAll()
        {
            return _engine.GetAll<Person>();
        }

        public long Insert(Person entity)
        {
            var result = _engine.Create(entity);
            return result != null ? result.Id : -1;
        }

        public void Update(Person entity)
        {
            _engine.Set(entity);
        }
    }
}
