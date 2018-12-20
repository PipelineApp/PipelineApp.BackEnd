// <copyright file="GraphDbClient.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces;
    using Neo4j.Driver.V1;
    using Providers;

    /// <inheritdoc />
    public class GraphDbClient : IGraphDbClient
    {
        private readonly ISession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphDbClient"/> class.
        /// </summary>
        /// <param name="graphDriver">The graph driver.</param>
        public GraphDbClient(IDriver graphDriver)
        {
            _session = graphDriver.Session();
        }

        /// <inheritdoc />
        public async Task<Fandom> CreateFandom(string name)
        {
            var id = Guid.NewGuid();
            var query = $"CREATE (v:{VertexTypes.FANDOM} {{ id: '{id}', name: '{name}' }}) RETURN v";
            return await _session.WriteTransactionAsync(async tx =>
            {
                var cursor = await tx.RunAsync(query);
                await cursor.FetchAsync();
                var fandom = new Fandom();
                fandom.LoadRecord(cursor.Current);
                return fandom;
            });
        }

        /// <inheritdoc />
        public int Count(string vertexType)
        {
            var query = $"MATCH (v:{vertexType}) RETURN count(*)";
            var data = _session.Run(query);
            return data.Peek().GetOrDefault("count(*)", 0);
        }

        /// <inheritdoc />
        public IEnumerable<T> GetAll<T>(string vertexType)
            where T : GraphEntity, new()
        {
            var query = $"MATCH (v:{vertexType}) RETURN v";
            var data = _session.Run(query);
            return data.Select(v =>
            {
                var entity = new T();
                entity.LoadRecord(v);
                return entity;
            });
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
                _session.CloseAsync();
            }
        }
    }
}
