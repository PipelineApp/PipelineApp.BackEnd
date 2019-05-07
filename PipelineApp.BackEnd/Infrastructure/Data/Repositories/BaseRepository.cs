﻿// <copyright file="BaseRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;
    using Entities;
    using Interfaces.Data;
    using Interfaces.Repositories;
    using Neo4jClient;
    using Relationships;
    using Requests;

    /// <inheritdoc cref="IRepository{TModel}"/>
    public class BaseRepository<TModel> : IRepository<TModel>
        where TModel : class, IEntity
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
        public TModel CreateWithRelationships(CreateNodeRequest<TModel> request)
        {
            using (var scope = new TransactionScope())
            {
                var id = request.Entity.Id;
                var node = GraphClient.Cypher.Create($"(e:{typeof(TModel).Name} {{model}})")
                    .WithParam("model", request.Entity)
                    .Return(e => e.As<TModel>())
                    .Results.FirstOrDefault();
                foreach (var rel in request.InboundRelationships)
                {
                    GraphClient.Cypher
                        .Match("(source)", "(target)")
                        .Where<BaseEntity>(source => source.Id == rel.SourceId)
                        .AndWhere((TModel target) => target.Id == id)
                        .Create($"(source)-[:{rel.GetType().Name}]->(target)")
                        .ExecuteWithoutResults();
                }
                foreach (var rel in request.OutboundRelationships)
                {
                    GraphClient.Cypher
                        .Match("(source)", "(target)")
                        .Where((TModel source) => source.Id == id)
                        .AndWhere<BaseEntity>(target => target.Id == rel.TargetId)
                        .Create($"(source)-[:{rel.GetType().Name}]->(target)")
                        .ExecuteWithoutResults();
                }

                scope.Complete();
                return node;
            }
        }

        /// <inheritdoc />
        public async Task AddOutboundRelationshipAsync<TRelationship, TTarget>(Guid sourceId, Guid targetId)
            where TRelationship : BaseRelationship
            where TTarget : BaseEntity
        {
            await GraphClient.Cypher
                .Match($"(source:{typeof(TModel).Name})", $"(target:{typeof(TTarget).Name})")
                .Where((TModel source) => source.Id == sourceId)
                .AndWhere((TTarget target) => target.Id == targetId)
                .Create($"(source)-[:{typeof(TRelationship).Name}]->(target)")
                .ExecuteWithoutResultsAsync();
        }

        /// <inheritdoc />
        public async Task RemoveOutboundRelationshipAsync<TRelationship, TTarget>(Guid sourceId, Guid targetId)
            where TRelationship : BaseRelationship
            where TTarget : BaseEntity
        {
            await GraphClient.Cypher
                .Match($"(source:{typeof(TModel).Name})-[r:{typeof(TRelationship).Name}]->(target:{typeof(TTarget).Name})")
                .Where((TModel source) => source.Id == sourceId)
                .AndWhere((TTarget target) => target.Id == targetId)
                .Delete("r")
                .ExecuteWithoutResultsAsync();
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
