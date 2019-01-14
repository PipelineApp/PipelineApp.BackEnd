// <copyright file="BaseRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;
    using Neo4j.Driver.V1;
    using Providers;

    /// <summary>
    /// Base class for all repository classes, which initializes graph client
    /// and manages disposal.
    /// </summary>
    /// <typeparam name="T">The type to and from which graph data should be mapped.</typeparam>
    public class BaseRepository<T> : IDisposable
        where T : GraphEntity, new()
    {
        /// <summary>
        /// Gets the graph database session.
        /// </summary>
        protected ISession Session { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="graphDriver">The graph driver.</param>
        public BaseRepository(IDriver graphDriver)
        {
            Session = graphDriver.Session();
        }

        /// <summary>
        /// Asynchronously runs a given graph database query and loads the result
        /// into objects of type <code>T</code>.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <returns>A list of objects of type <code>T</code> representing the result of the query.</returns>
        public async Task<IEnumerable<T>> LoadQuery(string query)
        {
            var cursor = await Session.RunAsync(query);
            var result = new List<T>();
            while (await cursor.FetchAsync())
            {
                var entity = new T();
                entity.LoadRecord(cursor.Current);
                result.Add(entity);
            }
            return result;
        }

        /// <summary>
        /// Asynchronously runs a given graph database query and loads the result
        /// into an object of type <code>T</code>.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <returns>An object of type <code>T</code> representing the result of the query.</returns>
        public async Task<T> LoadQuerySingle(string query)
        {
            var cursor = await Session.RunAsync(query);
            await cursor.FetchAsync();
            var entity = new T();
            entity.LoadRecord(cursor.Current);
            return entity;
        }

        /// <summary>
        /// Returns a count of all nodes of the given type in the database.
        /// </summary>
        /// <param name="vertexType">The vertex type to be counted.</param>
        /// <returns>An integer representing the node count.</returns>
        public int CountAll(string vertexType)
        {
            var query = $"MATCH (v:{vertexType}) RETURN count(*)";
            var data = Session.Run(query);
            return data.Peek().GetOrDefault("count(*)", 0);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Session.CloseAsync();
            }
        }
    }
}
