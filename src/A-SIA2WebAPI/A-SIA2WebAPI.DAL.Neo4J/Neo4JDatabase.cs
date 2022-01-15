using Neo4j.Driver;
using System;
using System.Collections.Generic;

namespace A_SIA2WebAPI.DAL.Neo4J
{
    public class Neo4JDatabase : IDisposable
    {
        private bool _disposed = false;
        private readonly IDriver _driver;

        ~Neo4JDatabase() => Dispose(false);

        public Neo4JDatabase()
        {
            _driver = GraphDatabase.Driver(
                Neo4JSettings.Uri,
                AuthTokens.Basic(
                    Neo4JSettings.Username,
                    Neo4JSettings.Password
                ));
        }

        public List<IRecord> RunQuery(Query query)
        {
            var session = _driver.AsyncSession();

            var transaction = session.BeginTransactionAsync().Result;

            var result = transaction.RunAsync(query).Result.ToListAsync().Result;

            transaction.CommitAsync().Wait();

            transaction.Dispose();

            session.CloseAsync().Wait();

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _driver?.Dispose();
            }

            _disposed = true;
        }
    }
}
