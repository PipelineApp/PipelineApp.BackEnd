// <copyright file="CreateNodeRequest.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Requests
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Data;
    using Relationships;

    /// <summary>
    /// Summary of a node to be created and its relationship to other nodes.
    /// </summary>
    /// <typeparam name="TModel">The type of the node to be created.</typeparam>
    public class CreateNodeRequest<TModel>
        where TModel : class, IEntity
    {
        /// <summary>
        /// Gets the node to be created.
        /// </summary>
        public TModel Entity { get; }

        /// <summary>
        /// Gets the inbound relationships which should be created leading to the new node.
        /// </summary>
        public List<BaseRelationship> InboundRelationships { get; }

        /// <summary>
        /// Gets the outbound relationships which should be created leading from the new node.
        /// </summary>
        public List<BaseRelationship> OutboundRelationships { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNodeRequest{TModel}"/> class.
        /// </summary>
        /// <param name="entity">The entity for which a new node should be created.</param>
        public CreateNodeRequest(TModel entity)
        {
            Entity = entity;
            Entity.Id = Guid.NewGuid();
            InboundRelationships = new List<BaseRelationship>();
            OutboundRelationships = new List<BaseRelationship>();
        }

        /// <summary>
        /// Adds a new inbound relationship to the set of objects to be created.
        /// </summary>
        /// <typeparam name="TRelationship">The type of the relationship to be created.</typeparam>
        /// <param name="sourceId">The ID of the node from which the relationship should originate.</param>
        /// <returns>The request.</returns>
        public CreateNodeRequest<TModel> WithInboundRelationship<TRelationship>(Guid sourceId)
            where TRelationship : BaseRelationship, new()
        {
            InboundRelationships.Add(new TRelationship { SourceId = sourceId });
            return this;
        }

        /// <summary>
        /// Adds a new outbound relationship to the set of objects to be created.
        /// </summary>
        /// <typeparam name="TRelationship">The type of the relationship to be created.</typeparam>
        /// <param name="targetId">The ID of the node to which the relationship should lead.</param>
        /// <returns>The request.</returns>
        public CreateNodeRequest<TModel> WithOutboundRelationship<TRelationship>(Guid targetId)
            where TRelationship : BaseRelationship, new()
        {
            OutboundRelationships.Add(new TRelationship { TargetId = targetId });
            return this;
        }
    }
}
