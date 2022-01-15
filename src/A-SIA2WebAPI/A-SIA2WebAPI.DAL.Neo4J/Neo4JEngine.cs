using A_SIA2WebAPI.Models;
using A_SIA2WebAPI.Models.Attributes;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace A_SIA2WebAPI.DAL.Neo4J
{
    public class Neo4JEngine
    {
        public Neo4JDatabase Database { get; }

        private readonly ILogger<Neo4JEngine> _logger;

        public Neo4JEngine(Neo4JDatabase database, ILogger<Neo4JEngine> logger)
        {
            _logger = logger;
            Database = database;
        }

        public IEnumerable<T> GetAll<T>() where T : Entity
        {
            Query query;
            // Relation
            if (IsRelation(typeof(T)))
            {
                // Check if there is an attrbiute for the type
                string? relName = typeof(T).GetCustomAttributes(true)
                    .Where(a => a is DatabaseRelationTypeAttribute)
                    .FirstOrDefault()?.As<DatabaseRelationTypeAttribute>().Type;

                query = new(
                @$"MATCH ()-[a{(relName == null ? "" : $":{relName}")}]-()
                RETURN DISTINCT a;");
            }
            // Node
            else
            {
                // Create query
                query = new(
                @$"MATCH (a: {typeof(T).Name})
                RETURN a;");
            }

            try
            {
                // Select
                var results = Database
                    .RunQuery(query);

                List<T> resultList = new();
                foreach (var r in results)
                {
                    try
                    {
                        var entity = ParseAs<T>(r["a"].As<IEntity>());
                        if (entity != null)
                            resultList.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"There was an error parsing an entity of type {nameof(T)} from the database result set!");
                    }
                }

                return resultList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was an error retrieving all of {nameof(T)}!");
                return Enumerable.Empty<T>();
            }
        }

        internal IEnumerable<T> GetAllRelations<T>(string relationType) where T : Relation
        {
            try
            {
                Query query = new(
                @$"MATCH ()-[a:{relationType}]-()
                RETURN DISTINCT a;");

                // Select
                var results = Database
                    .RunQuery(query);

                List<T> resultList = new();
                foreach (var r in results)
                {
                    try
                    {
                        var entity = ParseAs<T>(r["a"].As<IEntity>());
                        if (entity != null)
                            resultList.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"There was an error parsing an entity of type {nameof(T)} from the database result set!");
                    }
                }

                return resultList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was an error retrieving all of {nameof(T)}!");
                return Enumerable.Empty<T>();
            }
        }

        public T? Get<T>(long id) where T : Entity
        {
            Query query;
            // Relation
            if (IsRelation(typeof(T)))
            {
                query = new(
                @$"MATCH ()-[a]-()
                WHERE id(a)={id}
                RETURN a;");
            }
            // Node
            else
            {
                // Create query
                query = new(
                @$"MATCH (a)
                WHERE id(a)={id}
                RETURN a;");
            }

            try
            {
                // Select
                var result = Database
                    .RunQuery(query)
                    .First()["a"]
                    .As<IEntity>();

                return ParseAs<T>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was an error retrieving {nameof(T)}!");
                return null;
            }
        }

        public T? Create<T>(T obj) where T : Entity
        {
            IEntity? result = null;

            try
            {
                // Get parameters
                var parameters = GetParameters(obj, "n", out string setString);

                Query query;

                // Relationship
                Type type = typeof(T);
                if (obj is Relation rel)
                {
                    // Check if there is an attrbiute for the type
                    string? relName = typeof(T).GetCustomAttributes(true)
                        .Where(a => a is DatabaseRelationTypeAttribute)
                        .FirstOrDefault()?.As<DatabaseRelationTypeAttribute>().Type;

                    // Create query
                    query = new(
                        @$"MATCH (a), (b)
                    WHERE id(a)={rel.From} AND id(b)={rel.To}
                    CREATE (a)-[n:{relName ?? rel.RelationType}]->(b)
                    {setString}
                    RETURN n;");
                }
                // Node
                else
                {
                    // Create query
                    query = new(
                        @$"CREATE (n: {type.Name})
                    {setString}
                    RETURN n;");
                }


                // Add parameters
                foreach (var p in parameters)
                {
                    query.Parameters.Add(p);
                }

                // Get result
                result = Database
                    .RunQuery(query)
                    .First()["n"]
                    .As<IEntity>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was an error creating {nameof(T)} in the database!");
            }

            return ParseAs<T>(result);
        }

        public void Set<T>(T obj) where T : Entity
        {
            try
            {
                // Get parameters
                var parameters = GetParameters(obj, "a", out string setString);

                // Create query
                Query query;

                // TODO : SET HOTFIX FOR CHANGING FROM AND TO OF RELATION
                if (obj is Relation rel)
                {
                    // From and to of relation cannot be changed without creating a new relation
                    // Will maybe be implemented in the future
                    //query = new(
                    //    $@"MATCH ()-[r1]->(), (n1), (n2)
                    //    WHERE id(n1)={rel.From} AND id(n2)={rel.To} AND id(r1)={rel.Id}
                    //    CREATE (n1)-[r2:{rel.RelationType}]->(n2)
                    //    SET r2=r1
                    //    {(setString.Length > 0 ? $", {setString}" : "")}
                    //    DELETE r1;");

                    query = new(
                        $@"MATCH ()-[a]-()
                        WHERE id(a)={rel.Id}
                        {setString};");
                }
                else
                {
                    query = new(
                        @$"MATCH (a)
                        WHERE id(a)={obj.Id}
                        {setString};");
                }

                // Add parameters
                foreach (var p in parameters)
                {
                    query.Parameters.Add(p);
                }

                Database.RunQuery(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was an error setting the properties of {nameof(T)} in the database!");
            }
        }

        public void Delete<T>(long id) where T : Entity
        {
            try
            {
                Query query;
                if (IsRelation(typeof(T)))
                {
                    query = new(
                        @$"MATCH ()-[a]-()
                    WHERE id(a)={id}
                    DETACH DELETE a;");
                }
                else
                {
                    query = new(
                        @$"MATCH (a)
                    WHERE id(a)={id}
                    DETACH DELETE a;");
                }

                // Delete
                Database.RunQuery(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was an error deleting {nameof(T)} with id {id} in the database!");

            }
        }

        #region Helper Methods

        private static bool IsRelation<T>(T obj) where T : Entity
        {
            return IsRelation(obj.GetType());
        }
        private static bool IsRelation(Type t)
        {
            return t.GetProperty("RelationType") != null;
        }
        private static IDictionary<string, object> GetParameters<T>(T obj, string objQueryName, out string setString) where T : Entity
        {
            setString = "SET ";
            IDictionary<string, object> praramters = new Dictionary<string, object>();

            bool first = true;

            foreach (var property in obj.GetType().GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(false);

                if (attributes.Where(a => a is DatabasePropertyNameAttribute).FirstOrDefault() is DatabasePropertyNameAttribute attr)
                {
                    object? value = property.GetValue(obj);
                    if (value == null)
                        continue;

                    if (value is not string and not int and not long and not float and not double)
                    {
                        value = JsonConvert.SerializeObject(value);
                    }

                    if (first) { first = false; }
                    else { setString += ","; }

                    var key = $"p_{attr.PropertyName}";
                    praramters.Add(key, value);
                    setString += $" {objQueryName}.{attr.PropertyName}=${key} ";
                }
            }

            if (setString.Length == 4)
                setString = "";

            return praramters;
        }

        public object? ParseAs(IEntity? entity, Type type)
        {
            if (entity == null)
                return null;

            try
            {
                // Instantiate
                object? obj = Activator.CreateInstance(type);

                if (obj == null)
                    return null;

                // Change type
                obj = Convert.ChangeType(obj, type);

                // Set id
                type.GetProperty("Id")?.SetValue(obj, entity.Id);

                // Set relationship info
                var info = type.GetProperty("RelationType");
                if (info != null)
                {
                    IRelationship rel = entity.As<IRelationship>();
                    info.SetValue(obj, rel.Type);
                    type.GetProperty("From")?.SetValue(obj, rel.StartNodeId);
                    type.GetProperty("To")?.SetValue(obj, rel.EndNodeId);
                }

                // Set properties
                Dictionary<string, string> mapping = new();
                foreach (var property in type.GetProperties())
                {
                    object[] attributes = property.GetCustomAttributes(false);

                    if (attributes.Where(a => a is DatabasePropertyNameAttribute).FirstOrDefault()
                        is DatabasePropertyNameAttribute attr)
                    {
                        if (attr.PropertyName is string key)
                            mapping.Add(key, property.Name);
                    }
                }

                foreach (var property in entity.Properties)
                {
                    object? value = property.Value;
                    System.Reflection.PropertyInfo? propInfo = type.GetProperty(mapping[property.Key]);

                    if (propInfo == null)
                    {
                        _logger.LogWarning(
                            new NullReferenceException(),
                            $"The property {mapping[property.Key]} was not found on the object {type.Name}!");
                        continue;
                    }

                    try
                    {
                        propInfo.SetValue(obj, value);
                    }
                    catch
                    {
                        // Common error that single is saved as double
                        if (value is double dub)
                        {
                            value = (float)dub;
                        }
                        // Common error that int32 is saved as int64
                        else if (value is long lon)
                        {
                            value = (int)lon;
                        }
                        else if (value is string str)
                        {
                            value = JsonConvert.DeserializeObject(str, propInfo.PropertyType);
                        }

                        propInfo.SetValue(obj, value);
                    }
                }

                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was an error parsing the entity of type {type.Name} from the database!");
            }

            return default;
        }

        public T? ParseAs<T>(IEntity? entity) where T : Entity
        {
            return ParseAs(entity, typeof(T)) as T;
        }

        #endregion
    }
}
