﻿using A_SIA2WebAPI.DAL.Neo4J;
using A_SIA2WebAPI.DAL.Neo4J.Repositories;
using A_SIA2WebAPI.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Neo4j.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;

namespace A_SIA2WebAPI.DAL.Test.Neo4J.Repositories
{
    public class Neo4JNetworkRepositoryTest
    {
        private Network _entity;
        private Network _entity2;
        private Neo4JEngine _engine;
        private Neo4JNetworkRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _engine = new(
                new Neo4JDatabase(),
                new Mock<ILogger<Neo4JEngine>>().Object);

            _repository = new(
                _engine,
                new Mock<ILogger<Neo4JNetworkRepository>>().Object);

            _entity = new()
            {
                Name = "New Network",
                DefaultInfluence = 0,
                DefaultInterval = 1,
                DefaultNodevalue = 1,
                DefaultOffset = 0,
                DefaultPersistance = 1,
                DefaultReflection = 1,
                Description = "This is a new Network!",
                LabelType = 1,
                MaxNodevalue = 3,
                MinNodevalue = -3,
                Anonymous = false,
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
            long id = _repository.Insert(_entity);
            
            // Check that entity was inserted
            Assert.AreNotEqual(-1, id);

            _entity.Id = id;
            _entity.Description = "New Description";

            // Update
            _repository.Update(_entity);

            // Confirm update status of entity
            var updated = _repository.Get(id);

            Assert.AreEqual(_entity.Description, updated?.Description);
        }

        [Test]
        public void GetAllTest()
        {
            // Insert entities
            _entity.Id  = _repository.Insert(_entity);

            _entity2 = new()
            {
                Name = "New Network",
                DefaultInfluence = 0,
                DefaultInterval = 1,
                DefaultNodevalue = 1,
                DefaultOffset = 0,
                DefaultPersistance = 1,
                DefaultReflection = 1,
                Description = "This is a new Network!",
                LabelType = 1,
                MaxNodevalue = 3,
                MinNodevalue = -3,
                Anonymous = false,
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
                JsonConvert.SerializeObject(entities[0]));
            Assert.AreEqual(
                JsonConvert.SerializeObject(_entity2),
                JsonConvert.SerializeObject(entities[1]));
        }

        [TearDown]
        public void TearDown()
        {
            Query query = new("MATCH (n) WHERE id(n)=$id DETACH DELETE n;");
            query.Parameters.Add("id", _entity.Id);

            _engine.Database.RunQuery(query);

            if(_entity2 != null)
            {
                query.Parameters["id"] = _entity2.Id;
                _engine.Database.RunQuery(query);
            }
        }
    }
}
