// <copyright file="BaseRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces.Data;
    using Interfaces.Repositories;
    using Neo4jClient;
    using Relationships;

    /// <inheritdoc cref="IRepository{TModel}"/>
    public class BaseRepository<TModel> : IRepository<TModel>
        where TModel : IEntity
    {
        /// <summary>
        /// Gets the graph client.
        /// </summary>
        protected IGraphClient GraphClient { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TModel}"/> class.
        /// </summary>
        /// <param name="client">The graph client.</param>
        public BaseRepository(IGraphClient client)
        {
            GraphClient = client;
            GraphClient.Connect();
        }

        /// <inheritdoc />
        public async Task<IList<TModel>> GetAllAsync()
        {
            var match = GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})");
            var returnQuery = match.Return(e => e.As<TModel>());
            var results = await returnQuery.ResultsAsync;
            return results.ToList();
        }

        /// <inheritdoc />
        public async Task<TModel> GetByIdAsync(Guid id)
        {
            var results = await GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                                     .Where<TModel>(e => e.Id == id)
                                     .Return(e => e.As<TModel>())
                                     .ResultsAsync;
            return results.SingleOrDefault();
        }

        /// <inheritdoc />
        public async Task<TModel> SaveAsync(TModel model)
        {
            model.Id = Guid.NewGuid();
            var results = await GraphClient.Cypher.Create($"(e:{typeof(TModel).Name} {{model}})")
                                     .WithParam("model", model)
                                     .Return(e => e.As<TModel>())
                                     .ResultsAsync;
            return results.Single();
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithInboundRelationshipAsync<TRelationship, TSource>(TModel model, Guid sourceId)
            where TRelationship : BaseRelationship
            where TSource : BaseEntity
        {
            model.Id = Guid.NewGuid();
            var results = await GraphClient.Cypher
                .Match($"(source:{typeof(TSource).Name})")
                .Where((TSource source) => source.Id == sourceId)
                .Create($"(source)-[:{typeof(TRelationship).Name}]->(newEntity:{typeof(TModel).Name} {{model}})")
                .WithParam("model", model)
                .Return(newEntity => newEntity.As<TModel>())
                .ResultsAsync;
            return results.Single();
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAsync(TModel model)
        {
            var id = model.Id;

            var results = await GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                                     .Where<IEntity>(e => e.Id == id)
                                     .Set("e = {model}")
                                     .WithParam("model", model)
                                     .Return(e => e.As<TModel>())
                                     .ResultsAsync;
            return results.Single();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id)
        {
            await GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                              .Where<IEntity>(e => e.Id == id)
                              .DetachDelete("e")
                              .ExecuteWithoutResultsAsync();
        }

        /// <inheritdoc />
        public async Task<long> Count()
        {
            var results = await GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                .Return(e => e.Count())
                .ResultsAsync;
            return results.SingleOrDefault();
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
            GraphClient.Dispose();
        }
    }
}
