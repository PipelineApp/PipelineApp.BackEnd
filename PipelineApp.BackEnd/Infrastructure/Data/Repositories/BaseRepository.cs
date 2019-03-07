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
    using Interfaces.Data;
    using Interfaces.Repositories;
    using Neo4jClient;

    /// <inheritdoc cref="IRepository{TModel}"/>
    public class BaseRepository<TModel> : IRepository<TModel>
        where TModel : IEntity
    {
        private readonly IGraphClient _graphClient;

        /// <summary>
        /// Gets the graph client.
        /// </summary>
        protected IGraphClient GraphClient => _graphClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TModel}"/> class.
        /// </summary>
        /// <param name="client">The graph client.</param>
        public BaseRepository(IGraphClient client)
        {
            _graphClient = client;
            _graphClient.Connect();
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
            var results = await GraphClient.Cypher.Create($"(e:{typeof(TModel).Name} {{model}})")
                                     .WithParam("model", model)
                                     .Return(e => e.As<TModel>())
                                     .ResultsAsync;
            return results.Single();
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAsync(TModel model)
        {
            Guid id = model.Id;

            var results = await GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                                     .Where<IEntity>(e => e.Id == id)
                                     .Set("e = {model}")
                                     .WithParam("model", model)
                                     .Return(e => e.As<TModel>())
                                     .ResultsAsync;
            return results.Single();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(TModel model)
        {
            Guid id = model.Id;

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
