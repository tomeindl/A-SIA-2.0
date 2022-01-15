using A_SIA2WebAPI.DAL.Neo4J;
using A_SIA2WebAPI.DAL.Neo4J.Repositories;
using A_SIA2WebAPI.Models;
using A_SIA2WebAPI.Models.Attributes;
using Microsoft.Extensions.Logging;
using Moq;
using Neo4j.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;

namespace A_SIA2WebAPI.DAL.Test.Neo4J.Repositories
{
    public class Neo4JRelationRepositoryTest
    {
        private Relation _entity;
        private Relation _entity2;

        private INode _node1;
        private INode _node2;

        private Neo4JEngine _engine;
        private Neo4JRelationRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _engine = new(
                new Neo4JDatabase(),
                new Mock<ILogger<Neo4JEngine>>().Object);

            _repository = new(_engine);

            // Insert nodes to connect to
            Query query = new("CREATE (n1: TestNode) CREATE (n2: TestNode) RETURN n1, n2;");

            var result = _engine.Database.RunQuery(query);

            _node1 = result[0]["n1"].As<INode>();
            _node2 = result[0]["n2"].As<INode>();

            _entity = new()
            {
                From = _node1.Id,
                To = _node2.Id,
                RelationType = "TEST_RELATION"
            };
        }

        [Test]
        public void InsertTest()
        {
            long id = _repository.Insert(_entity);

            // Check that entity was inserted
            Assert.AreNotEqual(-1, id);

            // Retrieve inserted user
            var inserted = _repository.Get(id);
            if(inserted == null)
            {
                Assert.Fail("Get returned null, but should be user");
                return;
            }

            _entity.Id = inserted.Id;

            Assert.AreEqual(
                JsonConvert.SerializeObject(_entity),
                JsonConvert.SerializeObject(inserted));
        }

        [Test]
        public void DeleteTest()
        {
            _entity.Id = _repository.Insert(_entity);

            // Check that entity was inserted
            Assert.AreNotEqual(-1, _entity.Id);

            _repository.Delete(_entity.Id);

            // Confirm removal of entity
            var inserted = _repository.Get(_entity.Id);
            Assert.IsNull(inserted);
        }

        [Test]
        public void UpdateTest()
        {
            Assert.Throws<NotImplementedException>(() => _repository.Update(_entity));
        }

        [Test]
        public void GetAllTest()
        {
            // Insert entities
            _entity.Id  = _repository.Insert(_entity);

            _entity2 = new()
            {
                From = _node1.Id,
                To = _node2.Id,
                RelationType = "OTHER_TEST_RELATION"
            };
            _entity2.Id = _repository.Insert(_entity2);

            Assert.AreNotEqual(-1, _entity.Id);
            Assert.AreNotEqual(-1, _entity2.Id);

            // Get all
            var entities = _repository.GetAll().ToList();

            // Check length of collection
            Assert.AreEqual(2, entities.Count);
            // Check content of colelction
            Assert.AreEqual(
                JsonConvert.SerializeObject(_entity),
                JsonConvert.SerializeObject(entities[1]));
            Assert.AreEqual(
                JsonConvert.SerializeObject(_entity2),
                JsonConvert.SerializeObject(entities[0]));
        }

        [TearDown]
        public void TearDown()
        {
            // Delete nodes
            Query query = new("MATCH (n) WHERE id(n)=$id DETACH DELETE n;");

            if (_node1 != null)
            {
                query.Parameters["id"] = _node1.Id;
                _engine.Database.RunQuery(query);
            }
            if (_node2 != null)
            {
                query.Parameters["id"] = _node2.Id;
                _engine.Database.RunQuery(query);
            }
        }
    }
}
