using A_SIA2WebAPI.DAL.Common;
using A_SIA2WebAPI.Models;
using A_SIA2WebAPI.Models.Attributes;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace A_SIA2WebAPI.DAL.Neo4J.Repositories
{
    public class Neo4JNetworkStructureRepository : IRepository<NetworkStructure>
    {
        private readonly Neo4JEngine _engine;
        private readonly ILogger<Neo4JNetworkStructureRepository> _logger;

        public Neo4JNetworkStructureRepository(Neo4JEngine engine, ILogger<Neo4JNetworkStructureRepository> logger)
        {
            _engine = engine;
            _logger = logger;
        }

        public void Delete(long id)
        {
            throw new InvalidOperationException();
        }

        public NetworkStructure? Get(long networkId)
        {
            try
            {
                Network? network = _engine.Get<Network>(networkId);

                // Network not found
                if (network == null)
                    return null;

                var types = Assembly.GetAssembly(typeof(Entity))?
                    .GetTypes().Where(t => typeof(Entity).IsAssignableFrom(t)).ToList();

                if (types == null)
                    return null;

                // Get all nodes from network
                List<SocialNode> nodes = new();

                Query query = new(
                    @"MATCH (network: Network), (n)
                    WHERE id(network)=$network_id
                    AND (network)-[:NETWORK_CONTAINS]->(n)
                    RETURN n;");
                query.Parameters.Add("network_id", networkId);

                var result = _engine.Database.RunQuery(query);

                foreach (var node in result)
                {
                    if (node.Keys.Contains("n"))
                    {
                        INode n = node["n"].As<INode>();

                        Type? type = types.Find(t => n.Labels.Contains(t.Name));
                        if (type != null)
                        {
                            object? obj = _engine.ParseAs(n, type);

                            if (obj != null)
                                nodes.Add((SocialNode)obj); // TO BE REVIEWED
                        }
                    }
                }

                // Get all relations from network
                List<Relation> relationships = new();

                query = new(
                    @"MATCH (network: Network), (n)-[r]-()
                    WHERE id(network)=$network_id
                    AND (network)-[:NETWORK_CONTAINS]->(n)
                    RETURN DISTINCT r;");
                query.Parameters.Add("network_id", networkId);

                result = _engine.Database.RunQuery(query);

                foreach (var relationship in result)
                {
                    if (relationship.Keys.Contains("r"))
                    {
                        IRelationship r = relationship["r"].As<IRelationship>();

                        // Try to get type from mapping
                        Type? type = types.Where(t => t?.GetCustomAttribute<DatabaseRelationTypeAttribute>()?.Type == r.Type)
                            .FirstOrDefault();

                        // Catch any non mapped relation types and parse them as normal relations
                        if (type == null)
                            type = typeof(Relation);

                        // Add to collection
                        if (type != null)
                        {
                            object? obj = _engine.ParseAs(r, type);

                            if (obj != null)
                                relationships.Add((Relation)obj);
                        }
                    }
                }

                // Create network structure from data
                NetworkStructure networkStructure = new()
                {
                    People = new(),
                    Groups = new(),
                    InfluenceMatrix = new()
                };

                // Add people
                nodes.ForEach(node => { if (node is Person) networkStructure.People.Add(node.As<Person>()); });

                // Add groups
                nodes.ForEach(node =>
                {
                    if (node is Group)
                    {
                        networkStructure.Groups.Add(new()
                        {
                            Group = node.As<Group>(),
                            Nodes = nodes.Where(n => relationships
                            .Where(r => r.RelationType == "GROUP_CONTAINS" && r.To == n.Id && r.From == node.Id)
                            .Any()).ToList()
                        });
                    }
                });

                // Add influence matrix
                relationships.ForEach(rel =>
                {
                    if (rel is InfluencesRelation irel)
                    {
                        networkStructure.InfluenceMatrix.Add(new()
                        {
                            From = nodes.Find(n => n.Id == irel.From).As<Person>(),
                            To = nodes.Find(n => n.Id == irel.To).As<Person>(),
                            Relation = irel
                        });
                    }
                });

                return networkStructure;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error retrieving the NetworkStructure from neo4j database!");
            }

            return null;
        }

        public IEnumerable<NetworkStructure> GetAll()
        {
            throw new InvalidOperationException();
        }

        public long Insert(NetworkStructure entity)
        {
            throw new InvalidOperationException();
        }

        public void Update(NetworkStructure entity)
        {
            throw new InvalidOperationException();
        }
    }
}
