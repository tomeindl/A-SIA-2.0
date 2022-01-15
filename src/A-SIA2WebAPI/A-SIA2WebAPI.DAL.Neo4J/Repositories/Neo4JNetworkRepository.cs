using A_SIA2WebAPI.DAL.Common;
using A_SIA2WebAPI.Models;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace A_SIA2WebAPI.DAL.Neo4J.Repositories
{
    public class Neo4JNetworkRepository : IRepository<Network>
    {
        private readonly Neo4JEngine _engine;
        private readonly ILogger<Neo4JNetworkRepository> _logger;

        public Neo4JNetworkRepository(Neo4JEngine engine, ILogger<Neo4JNetworkRepository> logger)
        {
            _engine = engine;
            _logger = logger;
        }

        public void Delete(long id)
        {
            _engine.Delete<Network>(id);
        }

        public Network? Get(long id)
        {
            return _engine.Get<Network>(id);
        }

        public IEnumerable<Network> GetAllUserNetworks(long userId)
        {
            try
            {
                Query query = new(
                    @"MATCH (n: Network), (u: User)
                    WHERE id(u)=$user_id
                    AND (u)-[r: OWNS_NETWORK]->(n)
                    RETURN n;");
                query.Parameters.Add("user_id", userId);

                var results = _engine.Database.RunQuery(query);
                List<Network> networks = new();

                foreach (var result in results)
                {
                    try
                    {
                        Network? network = _engine.ParseAs<Network>(result.As<INode>());
                        if (network != null)
                            networks.Add(network);
                    }
                    catch
                    {
                        continue;
                    }
                }

                return networks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while getting networks of user {userId}!");
                return Enumerable.Empty<Network>();
            }
        }

        public IEnumerable<Network> GetAll()
        {
            return _engine.GetAll<Network>();
        }

        public long Insert(Network entity)
        {
            var result = _engine.Create(entity);
            return result != null ? result.Id : -1;
        }

        public void Update(Network entity)
        {
            _engine.Set(entity);
        }
    }
}
