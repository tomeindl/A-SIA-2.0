using A_SIA2WebAPI.DAL.Common;
using A_SIA2WebAPI.Models;
using System.Collections.Generic;

namespace A_SIA2WebAPI.DAL.Neo4J.Repositories
{
    public class Neo4JInfluencesRelationRepository : IRepository<InfluencesRelation>
    {
        private readonly Neo4JEngine _engine;

        public Neo4JInfluencesRelationRepository(Neo4JEngine engine)
        {
            _engine = engine;
        }

        public void Delete(long id)
        {
            _engine.Delete<InfluencesRelation>(id);
        }

        public InfluencesRelation? Get(long id)
        {
            return _engine.Get<InfluencesRelation>(id);
        }

        public IEnumerable<InfluencesRelation> GetAll()
        {
            return _engine.GetAll<InfluencesRelation>();
        }

        public long Insert(InfluencesRelation entity)
        {
            var result = _engine.Create(entity);
            return result != null ? result.Id : -1;
        }

        /// <summary>
        /// <b>IMPORTANT NOTICE</b><br></br>
        /// Only the properties will be changed on update! <u>The
        /// start node, end node and relation-type wont be updated</u>!
        /// If these parameter should be updated, create a new
        /// relation and delete the old one.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(InfluencesRelation entity)
        {
            _engine.Set(entity);
        }
    }
}
