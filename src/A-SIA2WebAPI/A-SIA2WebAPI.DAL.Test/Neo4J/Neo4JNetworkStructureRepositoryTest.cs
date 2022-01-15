using A_SIA2WebAPI.DAL.Neo4J;
using A_SIA2WebAPI.DAL.Neo4J.Repositories;
using A_SIA2WebAPI.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace A_SIA2WebAPI.DAL.Test.Neo4J
{
    public class Neo4JNetworkStructureRepositoryTest
    {
        private Neo4JEngine _engine;
        private Mock<ILogger<Neo4JEngine>> _engineLogger;
        private Mock<ILogger<Neo4JNetworkStructureRepository>> _repoLogger;
        private Neo4JNetworkStructureRepository _repo;

        [SetUp]
        public void Setup()
        {
            _engineLogger = new();
            _engine = new(new Neo4JDatabase(), _engineLogger.Object);

            _repoLogger = new();
            _repo = new(_engine, _repoLogger.Object);
        }

        [Test]
        public void CreateSinglePersonTest()
        {
            // Arrange
            Network? network = new()
            {
                Name = "New Network"
            };
            network = _engine.Create(network);

            Person? person = new()
            {
                Name = "New Person",
                Description = "This is a new Person!",
                Color = "#FFFFFF",
                Persistance = 2,
                PositionX = 3.214f,
                PositionY = -5.224f,
                Reflexion = 2,
                SimulationValues = new()
            };
            person = _engine.Create(person);
            var containsRel = new Relation()
            {
                From = network.Id,
                To = person.Id,
                RelationType = "NETWORK_CONTAINS"
            };
            _engine.Create(containsRel);

            // Act - Parse structure
            NetworkStructure? networkStructure = _repo.Get(network.Id);

            // Cleanup
            _engine.Delete<Person>(person.Id);
            _engine.Delete<Network>(network.Id);

            // Assert
            Assert.NotNull(networkStructure);
            Assert.AreEqual(1, networkStructure?.People.Count);
            Assert.AreEqual(0, networkStructure?.Groups.Count);
            Assert.AreEqual(0, networkStructure?.InfluenceMatrix.Count);
        }

        [Test]
        public void CreateMultiplePersonRelationshipsTest()
        {
            // Arrange
            Network? network = new()
            {
                Name = "New Network"
            };
            network = _engine.Create(network);

            Assert.NotNull(network);

            List<Person?> people = new();
            for (int i = 0; i < 10; i++)
            {
                Person? p = new()
                {
                    Name = $"New Person {i}",
                    Description = "This is a new Person!",
                    Color = "#FFFFFF",
                    Persistance = 2,
                    PositionX = 3.214f,
                    PositionY = -5.224f,
                    Reflexion = 2,
                    SimulationValues = new()
                };
                p = _engine.Create(p);
                people.Add(p);
                
                // Add person to network
                _engine.Create(new Relation()
                {
                    From = network.Id,
                    To = p.Id,
                    RelationType = "NETWORK_CONTAINS"
                });
            }

            List<InfluencesRelation?> relations = new();
            for (int i = 0; i < people.Count - 1; i++)
            {
                Person? p1 = people[i];
                Person? p2 = people[i + 1];
                if (p1 != null && p2 != null)
                {
                    InfluencesRelation rel = new()
                    {
                        From = p1.Id,
                        To = p2.Id,
                        Influence = 2,
                        Interval = 1,
                        Offset = 5
                    };
                    relations.Add(_engine.Create(rel));
                }
            }
            // Act - Parse structure
            NetworkStructure? networkStructure = _repo.Get(network.Id);

            // Cleanup
            _engine.Delete<Network>(network.Id);
            foreach (var p in people)
            {
                _engine.Delete<Person>(p.Id);
            }
            foreach (var r in relations)
            {
                _engine.Delete<Relation>(r.Id);
            }

            // Assert
            Assert.NotNull(networkStructure);
            Assert.AreEqual(people.Count, networkStructure?.People.Count);
            Assert.AreEqual(0, networkStructure?.Groups.Count);
            Assert.AreEqual(relations.Count, networkStructure?.InfluenceMatrix.Count);
        }

        [Test]
        public void CreateGroupsTest()
        {
            // Arrange
            Network? network = new()
            {
                Name = "New Network"
            };
            network = _engine.Create(network);

            Assert.NotNull(network);

            List<Person?> people = new();
            for (int i = 0; i < 10; i++)
            {
                Person? p = new()
                {
                    Name = $"New Person {i}",
                    Description = "This is a new Person!",
                    Color = "#FFFFFF",
                    Persistance = 2,
                    PositionX = 3.214f,
                    PositionY = -5.224f,
                    Reflexion = 2,
                    SimulationValues = new()
                };
                p = _engine.Create(p);
                people.Add(p);

                // Add person to network
                _engine.Create(new Relation()
                {
                    From = network.Id,
                    To = p.Id,
                    RelationType = "NETWORK_CONTAINS"
                });
            }

            List<Group?> groups = new();
            for (int i = 0; i < people.Count / 5; i++)
            {
                Group? g = new()
                {
                    Name = $"New Group {i}",
                    Description = "This is a new Group!",
                    Collapsed = true,
                    Color = "#FFFFFF",
                    Persistance = 2,
                    PositionX = 3.214f,
                    PositionY = -5.224f,
                    Reflexion = 2,
                    SimulationValues = new(),
                };

                g = _engine.Create(g);

                groups.Add(g);

                // Add group to network
                _engine.Create(new Relation()
                {
                    From = network.Id,
                    To = g.Id,
                    RelationType = "NETWORK_CONTAINS"
                });
            }

            var rand = new System.Random();
            foreach (var p in people)
            {
                Relation? r = new()
                {
                    From = groups[rand.Next(0, groups.Count)].Id,
                    To = p.Id,
                    RelationType = "GROUP_CONTAINS"
                };

                _engine.Create(r);
            }

            List<InfluencesRelation?> relations = new();
            for (int i = 0; i < people.Count - 1; i++)
            {
                Person? p1 = people[i];
                Person? p2 = people[i + 1];
                if (p1 != null && p2 != null)
                {
                    InfluencesRelation rel = new()
                    {
                        From = p1.Id,
                        To = p2.Id,
                        Influence = 2,
                        Interval = 1,
                        Offset = 5
                    };
                    relations.Add(_engine.Create(rel));
                }
            }
            // Act - Parse structure
            NetworkStructure? networkStructure = _repo.Get(network.Id);

            // Cleanup
            _engine.Delete<Network>(network.Id);
            foreach (var p in people)
            {
                _engine.Delete<Person>(p.Id);
            }
            foreach (var r in relations)
            {
                _engine.Delete<Relation>(r.Id);
            }
            foreach (var g in groups)
            {
                _engine.Delete<Group>(g.Id);
            }

            // Assert
            Assert.NotNull(networkStructure);
            Assert.AreEqual(people.Count, networkStructure?.People.Count);
            Assert.AreEqual(people.Count / 5, networkStructure?.Groups.Count);
            Assert.AreEqual(relations.Count, networkStructure?.InfluenceMatrix.Count);
        }
    }
}
