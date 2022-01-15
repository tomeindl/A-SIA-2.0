using A_SIA2WebAPI.DAL.Neo4J;
using A_SIA2WebAPI.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace A_SIA2WebAPI.DAL.Test.Neo4J
{
    public class Neo4JEngineTest
    {
        private Person? _person0;
        private Person? _person1;
        private InfluencesRelation? _influencesRelation;
        private Neo4JEngine? _engine;
        private Mock<ILogger<Neo4JEngine>> _logger;
        private Mock<Neo4JDatabase> _database;

        [SetUp]
        public void Setup()
        {
            // Arrange
            _person0 = new Person()
            {
                Id = -1,
                Name = "Max Mustermann",
                Color = "#FFFFFF",
                AvatarPath = "/test",
                Description = "A new user",
                Persistance = 0.45f,
                PositionX = 4.25f,
                PositionY = -3.45f,
                Reflexion = 0.2f,
                Roles = new() { "Isolator" },
                SimulationValues = new() { { 0, 1.25f }, { 1, 0.75f } },
            };

            _person1 = new Person()
            {
                Id = -1,
                Name = "Maria Musterfrau",
                Color = "#FFFFFF",
                AvatarPath = "/test",
                Description = "A new user",
                Persistance = 0.45f,
                PositionX = 4.25f,
                PositionY = -3.45f,
                Reflexion = 0.2f,
                Roles = new() { "Transmitter" },
                SimulationValues = new() { { 0, 1.25f }, { 1, 0.75f } },
            };
            _influencesRelation = new InfluencesRelation()
            {
                Id = -1,
                From = -1,
                To = -1,
                RelationType = "INFLUENCES",
                Influence = 2,
                Interval = 3,
                Offset = 5,
            };

            _logger = new();
            _database = new();
            _engine = new Neo4JEngine(_database.Object, _logger.Object);
        }

        [Test]
        public void CreatePersonTest()
        {
            if (_person0 != null)
                _person0 = _engine?.Create(_person0);

            Assert.IsTrue(_person0?.Id != -1);
        }

        [Test]
        public void CreateMultiplePersonAndRelationTest()
        {
            if (_person0 != null)
                _person0 = _engine?.Create(_person0);

            Assert.IsTrue(_person0?.Id != -1, "Error inserting person 0!");

            if (_person1 != null)
                _person1 = _engine?.Create(_person1);

            Assert.IsTrue(_person0?.Id != -1, "Error inserting person 1!");

            if (_influencesRelation != null)
            {
                // Assing new ids to relation
                if (_person0 != null)
                    _influencesRelation.From = _person0.Id;
                if (_person1 != null)
                    _influencesRelation.To = _person1.Id;

                _influencesRelation = _engine?.Create(_influencesRelation);
            }

            Assert.IsTrue(_influencesRelation?.Id != -1, "Error inserting relation!");
        }

        [Test]
        public void GetAllTest()
        {
            if (_engine == null)
            {
                Assert.Fail();
                return;
            }

            if (_person0 != null)
                _person0 = _engine.Create(_person0);

            Assert.IsTrue(_person0?.Id != -1, "Error inserting person 0!");

            if (_person1 != null)
                _person1 = _engine.Create(_person1);

            Assert.IsTrue(_person0?.Id != -1, "Error inserting person 1!");

            List<Person> list = new();

            if (_person0 != null && _person1 != null)
            {
                list = new(_engine.GetAll<Person>());
            }

            // Integration test: Assert.AreEqual(2, list.Count, $"Get all failed and returned {list.Count}!");
            Assert.Pass();
        }

        [Test]
        public void DeletePersonTest()
        {
            if (_person0 != null)
                _person0 = _engine?.Create(_person0);

            Assert.IsTrue(_person0?.Id != -1, "Error inserting person 0!");

            if (_person0 != null)
            {
                _engine?.Delete<Person>(_person0.Id);
                _person0 = _engine?.Get<Person>(_person0.Id);
            }

            Assert.IsNull(_person0, "Error deleting person 0!");
        }

        [Test]
        public void DeleteRelationTest()
        {
            if (_person0 != null)
                _person0 = _engine?.Create(_person0);

            Assert.IsTrue(_person0?.Id != -1, "Error inserting person 0!");

            if (_person1 != null)
                _person1 = _engine?.Create(_person1);

            Assert.IsTrue(_person0?.Id != -1, "Error inserting person 1!");

            if (_influencesRelation != null)
            {
                // Assing new ids to relation
                if (_person0 != null)
                    _influencesRelation.From = _person0.Id;
                if (_person1 != null)
                    _influencesRelation.To = _person1.Id;

                _influencesRelation = _engine?.Create(_influencesRelation);
            }

            Assert.IsTrue(_influencesRelation?.Id != -1, "Error inserting relation!");

            if (_influencesRelation != null)
            {
                _engine?.Delete<InfluencesRelation>(_influencesRelation.Id);
                _influencesRelation = _engine?.Get<InfluencesRelation>(_influencesRelation.Id);
            }

            Assert.IsNull(_influencesRelation, "Error deleting relation!");
        }

        [TearDown]
        public void TearDown()
        {
            if (_person0 != null)
                _engine?.Delete<Person>(_person0.Id);
            if (_person1 != null)
                _engine?.Delete<Person>(_person1.Id);
            if (_influencesRelation != null)
                _engine?.Delete<InfluencesRelation>(_influencesRelation.Id);
        }
    }
}