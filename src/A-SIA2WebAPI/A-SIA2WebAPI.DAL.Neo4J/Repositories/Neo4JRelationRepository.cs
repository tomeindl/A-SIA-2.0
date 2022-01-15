using A_SIA2WebAPI.DAL.Common;
using A_SIA2WebAPI.Models;
using System;
using System.Collections.Generic;

namespace A_SIA2WebAPI.DAL.Neo4J.Repositories
{
    public class Neo4JRelationRepository : IRepository<Relation>
    {
        private readonly Neo4JEngine _engine;

        public Neo4JRelationRepository(Neo4JEngine engine)
        {
            _engine = engine;
        }

        public void Delete(long id)
        {
            _engine.Delete<Relation>(id);
        }

        public Relation? Get(long id)
        {
            return _engine.Get<Relation>(id);
        }

        public IEnumerable<Relation> GetAll()
        {
            return _engine.GetAll<Relation>();
        }

        public IEnumerable<Relation> GetAll(string relationName)
        {
            return _engine.GetAllRelations<Relation>("relationName");
        }

        public long Insert(Relation entity)
        {
            var result = _engine.Create(entity);
            return result != null ? result.Id : -1;
        }

        /// <summary>
        /// The update method is not implemented yet for relations. 
        /// A problem within neo4j exists, where for every update of
        /// start, end or type of relationship, a new relationship
        /// must be instantiated and the id gets changed. Therefore,
        /// if the node must be updated, the developer has to manually
        /// insert a new node and delete the old one until further notice.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Update(Relation entity)
        {
            throw new NotImplementedException();
        }
    }
}
