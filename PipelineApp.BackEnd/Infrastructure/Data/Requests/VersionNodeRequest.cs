// <copyright file="CreateVersionNodeRequest.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Requests
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Interfaces.Data;
    using Relationships;

    /// <summary>
    /// Summary of a versioned node to be created and its relationship to other nodes.
    /// </summary>
    /// <typeparam name="TModel">The type of the node to be created.</typeparam>
    public class VersionNodeRequest<TModel> : CreateNodeRequest<VersionEntity<TModel>>
        where TModel : class, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionNodeRequest{TModel}"/> class.
        /// </summary>
        /// <param name="baseId">The ID of the base node to be versioned.</param>
        public VersionNodeRequest(Guid baseId)
            : base(new VersionEntity<TModel>())
        {
            WithInboundRelationshipFrom<HasVersion>(baseId);
        }
    }
}
